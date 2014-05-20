using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Transformalize.Configuration.Builders;
using Transformalize.Main;
using Transformalize.Main.Providers;

namespace DataProfiler {

    public class TableImporter {

        public Result Import(string resource) {

            AbstractConnection input;

            try {
                var userDefined = ProcessFactory.Create("DataProfiler")[0];
                input = userDefined.Connections["input"];
            } catch {
                throw new DataProfilerException("You must define a DataProfiler process with an 'input' connection in the transformalize configuration section.");
            }

            var modifier = new ConnectionModifier(resource);
            modifier.Modify(ref input);
            
            var builder = new ProcessBuilder(Regex.Replace(modifier.Name, "[^a-zA-Z]", string.Empty))
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
                    .Schema(modifier.Schema);

            var fields = new List<Field>();

            var abstractConnection = ProcessFactory.Create(builder.Process())[0].Connections["input"];
            var entitySchema = abstractConnection.GetEntitySchema(modifier.Name, modifier.Schema);
            if (entitySchema != null) {
                foreach (var field in entitySchema.Fields) {
                    builder
                        .Field(field.Name)
                        .Length(field.Length)
                        .Type(field.SimpleType)
                        .PrimaryKey(field.FieldType == FieldType.PrimaryKey);
                    fields.Add(new Field(field.Name, field.SimpleType, field.Length));
                }
            }

            var result = new Result {
                Fields = fields,
                Rows = ProcessFactory.Create(builder.Process())[0].Run()[modifier.Name],
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