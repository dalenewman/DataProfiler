#region license
// DataProfiler.Autofac
// Copyright 2013 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Cfg.Net.Ext;
using Pipeline.Configuration;
using Pipeline.Contracts;
using Pipeline.Ioc.Autofac.Modules;
using Pipeline.Logging.NLog;
using AdoModule = DataProfiler.Autofac.Modules.AdoModule;
using FileSchemaModule = DataProfiler.Autofac.Modules.FileSchemaModule;

namespace DataProfiler.Autofac {
    public class DataProfileModule : Module {
        readonly Connection _connection;

        public DataProfileModule(Connection connection) {
            _connection = connection;
        }

        protected override void Load(ContainerBuilder builder) {

            var cfg = new Process {
                Name = "Inspection",
                Mode = "meta",
                Connections = new List<Connection> { _connection },
                Entities = new List<Entity> { new Entity { Name = "Schema" }.WithDefaults() }
            }.WithDefaults().Serialize();

            var process = new Process();
            process.Load(cfg);

            builder.Register(ctx => new NLogPipelineLogger("DataProfiler", LogLevel.Info)).As<IPipelineLogger>().InstancePerLifetimeScope();

            builder.RegisterModule(new ContextModule(process));
            builder.RegisterModule(new AdoModule(process));
            builder.RegisterModule(new FileSchemaModule(process));

            builder.Register<IRunTimeRun>(ctx => new RunTimeRunner(ctx.ResolveNamed<IContext>(process.Key))).As<IRunTimeRun>();
            builder.Register<IImporter>(ctx => {
                var key = process.Connections.First(c => c.Name == "input").Key;
                return new Importer(ctx.ResolveNamed<ISchemaReader>(key), ctx.Resolve<IRunTimeRun>());
            }).As<IImporter>();
            builder.RegisterType<Profiler>().As<IProfiler>();
        }
    }
}
