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
using System.Linq;
using Autofac;
using Pipeline;
using Pipeline.Configuration;
using Pipeline.Contracts;
using Pipeline.Desktop;
using Pipeline.Desktop.Writers;
using Pipeline.Provider.Ado;

namespace DataProfiler.Autofac.Modules {
    public class EntityOutputModule : EntityModule {
        public EntityOutputModule(Process process) : base(process) { }

        public override void LoadEntity(ContainerBuilder builder, Process process, Entity entity) {

            builder.Register(ctx => {
                var output = ctx.ResolveNamed<OutputContext>(entity.Key);
                var rowFactory = ctx.ResolveNamed<IRowFactory>(entity.Key, new NamedParameter("capacity", output.GetAllEntityFields().Count()));
                var cf = ctx.ResolveNamed<IConnectionFactory>(output.Connection.Key);
                switch (output.Connection.Provider) {
                    case "sqlite":
                        return new TypedEntityMatchingKeysReader(new AdoEntityMatchingKeysReader(output, cf, rowFactory), output);
                    default:
                        return (ITakeAndReturnRows)new AdoEntityMatchingKeysReader(output, cf, rowFactory);
                }
            }).Named<ITakeAndReturnRows>(entity.Key);

            builder.Register<IWrite>(ctx => {
                var output = ctx.ResolveNamed<OutputContext>(entity.Key);
                return new ConsoleWriter(new JsonNetSerializer(output));
            }).Named<IWrite>(entity.Key);
        }

    }
}