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
using System.Collections.Generic;
using System.Linq;
using Transformalize;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Nulls;

namespace DataProfiler {

    public class Importer : IImporter {

        private readonly ISchemaReader _reader;
        private readonly IRunTimeRun _runner;
        private readonly IContext _context;

        public Importer(ISchemaReader reader, IRunTimeRun runner, IContext context) {
            _reader = reader;
            _runner = runner;
            _context = context;
        }

        public ImportResult Import(Connection connection) {

            if (_reader is NullSchemaReader) {
                _context.Error($"Unable to read schema of {connection}.");
                return new ImportResult {
                    Connection = connection,
                    Fields = new List<Field>(),
                    Rows = Enumerable.Empty<IRow>(),
                    Schema = null
                };
            }

            var schema = _reader.Read();

            var entity = schema.Entities.First();
            foreach (var field in entity.Fields.Where(f => Constants.InvalidFieldNames.Contains(f.Name)).Where(field => field.Alias == field.Name)) {
                field.Alias = field.Name + "Source";
                _context.Warn($"Reserved column name {field.Name} aliased as {field.Alias}.");
            }

            var cfg = new Process {
                Name = "Import",
                Connections = new List<Connection> { schema.Connection },
                Entities = new List<Entity> { entity }
            }.Serialize();

            var process = new Process();
            process.Load(cfg);

            if (process.Errors().Any()) {
                foreach (var error in process.Errors()) {
                    _context.Error(error);
                }
                return new ImportResult {
                    Connection = schema.Connection,
                    Fields = new List<Field>(),
                    Rows = Enumerable.Empty<IRow>(),
                    Schema = schema
                };
            }

            return new ImportResult {
                Connection = schema.Connection,
                Fields = process.Entities.First().Fields.Where(f => !f.System).ToList(),
                Schema = schema,
                Rows = _runner.Run(process),
            };
        }

    }
}