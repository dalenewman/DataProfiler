#region license
// Data Profiler
// Copyright © 2013-2018 Dale Newman
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
using System;
using System.IO;
using System.Linq;
using Autofac;
using Transformalize;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Extensions;
using Transformalize.Nulls;
using Transformalize.Providers.Excel;
using Transformalize.Providers.File;

namespace DataProfiler.Autofac.Modules {
    public class FileSchemaModule : Module {
        private readonly Process _process;
        public FileSchemaModule(Process process) {
            _process = process;
        }

        protected override void Load(ContainerBuilder builder) {
            foreach (var connection in _process.Connections.Where(c => c.Provider.In("file", "excel"))) {
                builder.Register<ISchemaReader>(ctx => {

                    /* file and excel are different, have to load the content and check it to determine schema */
                    var fileInfo = new FileInfo(Path.IsPathRooted(connection.File) ? connection.File : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, connection.File));
                    var context = ctx.ResolveNamed<IConnectionContext>(connection.Key);

                    if (fileInfo.Exists) {

                        var cfg = connection.Provider == "file" ? new FileInspection(context, fileInfo, 100).Create() : new ExcelInspection(context, fileInfo, 100).Create();

                        var process = new Process();
                        process.Load(cfg);

                        foreach (var warning in process.Warnings()) {
                            context.Warn(warning);
                        }

                        if (process.Errors().Any()) {
                            foreach (var error in process.Errors()) {
                                context.Error(error);
                            }
                            return new NullSchemaReader();
                        }

                        return new SchemaReader(context, new RunTimeRunner(context), process);
                    }

                    context.Error($"The file {fileInfo.FullName} does not exist.");
                    return new NullSchemaReader();
                }).Named<ISchemaReader>(connection.Key);
            }
        }
    }
}