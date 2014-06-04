using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Transformalize.Configuration.Builders;
using Transformalize.Libs.NLog;
using Transformalize.Main;
using Transformalize.Main.Providers;

namespace DataProfiler {

    public class TableImporter {

        private readonly Logger _logger = LogManager.GetLogger("DataProfiler.TableImporter");

        public Result Import(string resource, decimal sample = 100m) {

            AbstractConnection input;
            bool noLock;

            try {
                var userDefined = ProcessFactory.Create("DataProfiler")[0];
                input = userDefined.Connections["input"];
                sample = sample > 0m && sample < 100m ? sample : userDefined.Entities[0].Sample;
                noLock = userDefined.Entities[0].NoLock;
                _logger.Info("Sample: {0:###} percent, NoLock: {1}", sample, noLock);
            } catch {
                throw new DataProfilerException("You must define a DataProfiler process with an 'input' connection in the transformalize configuration section.");
            }

            var modifier = new ConnectionModifier(resource);
            modifier.Modify(ref input);

            var cleanName = Regex.Replace(modifier.Name, "[^a-zA-Z]", string.Empty);

            var builder = new ProcessBuilder("Dp" + cleanName[0].ToString(CultureInfo.InvariantCulture).ToUpper() + cleanName.Substring(1))
                .Connection("input")
                    .Provider(input.Type)
                    .Server(input.Server)
                    .Database(input.Database)
                    .User(input.User)
                    .Password(input.Password)
                    .Port(input.Port)
                    .ConnectionString(input.GetConnectionString())
                .Connection("output")
                    .Provider("internal")
                .Entity(modifier.Name)
                    .DetectChanges(false)
                    .NoLock(noLock)
                    .Sample(sample)
                    .Schema(modifier.Schema);

            var fields = new List<Field>();

            var abstractConnection = ProcessFactory.Create(builder.Process())[0].Connections["input"];
            var entitySchema = abstractConnection.GetEntitySchema(modifier.Name, modifier.Schema);
            if (entitySchema != null) {
                foreach (Transformalize.Main.Field field in entitySchema.Fields) {
                    builder
                        .Field(field.Name)
                        .Length(field.Length)
                        .Type(field.SimpleType);
                    fields.Add(new Field(field.Name, field.SimpleType, field.Length));
                }
            }

            var result = new Result {
                Name = modifier.Name,
                Fields = fields,
                Rows = ProcessFactory.Create(builder.Process())[0].Execute()[modifier.Name],
                Provider = input.Type.ToString()
            };
            result.Properties["server"] = input.Server;
            result.Properties["database"] = input.Database;
            result.Properties["schema"] = modifier.Schema;
            result.Properties["table"] = modifier.Name;
            result.Properties["port"] = input.Port.ToString(CultureInfo.InvariantCulture);

            return result;
        }

    }
}