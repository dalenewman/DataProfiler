using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Transformalize.Configuration.Builders;
using Transformalize.Libs.DBDiff.Schema.SqlServer2005.Model;
using Transformalize.Libs.Lucene.Net.Store;
using Transformalize.Libs.Nest.Domain.Similarity;
using Transformalize.Logging;
using Transformalize.Main;
using Transformalize.Main.Providers;

namespace DataProfiler {
    public class TableImporter : IImporter {

        public Result Import(string resource, decimal sample = 100m) {

            AbstractConnection input;
            bool noLock;

            ConnectionModifier modifier;

            try {
                var userDefined = ProcessFactory.CreateSingle("DataProfiler", new Options() { Mode = "metadata"});
                input = userDefined.Connections["input"];
                modifier = new ConnectionModifier(resource);
                modifier.Modify(ref input);

                var hasEntity = userDefined.Entities.Any();

                sample = sample > 0m && sample < 100m ?
                    sample :
                    hasEntity ? userDefined.Entities[0].Sample : 100m;

                noLock = !hasEntity || userDefined.Entities[0].NoLock;
                TflLogger.Info(userDefined.Name, modifier.Name, "Sample: {0:###} percent, NoLock: {1}", sample, noLock);
            } catch {
                throw new DataProfilerException("You must define a DataProfiler process with an 'input' connection in the transformalize configuration section.");
            }

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

            var process = ProcessFactory.CreateSingle(builder.Process());

            var result = new Result {
                Name = modifier.Name,
                Fields = process.Entities[0].Fields,
                Rows = process.Execute(),
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