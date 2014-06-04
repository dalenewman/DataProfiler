using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Transformalize.Configuration.Builders;
using Transformalize.Libs.Rhino.Etl;
using Transformalize.Main;

namespace DataProfiler {

    public class Profiler {

        public Dictionary<string, Row> Profile(string input, decimal sample = 100m) {
            var result = IsValidFileName(input) && new FileInfo(input).Exists ?
                new FileImporter().Import(input, sample) :
                new TableImporter().Import(input, sample);
            return Profile(result);
        }

        public Dictionary<string, Row> Profile(Result result) {

            var i = 0;
            var profile = result.Fields.ToDictionary(field => field.Name, field => new Row() {
                { "field", field.Name },
                { "type", field.Type},
                { "index", ++i}
            });

            var aggregates = new Dictionary<string, bool> {
                {"min", false},
                {"max", false},
                {"minlength", false},
                {"maxlength", false},
                {"count", true}
            };

            foreach (var pair in aggregates) {
                AddToProfile(ref profile, result, pair.Key, pair.Value);
            }

            return profile;
        }

        private static Row GetBuilder(Result result, string aggregate, bool distinct)
        {
            var processName = "Dp" + result.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper() + result.Name.Substring(1);
            var builder = new ProcessBuilder(processName)
                .Star(aggregate)
                .Connection("input")
                    .Provider("internal")
                .Connection("output")
                    .Provider("internal")
                .Entity(aggregate)
                    .DetectChanges(false)
                    .InputOperation(new RowsOperation(result.Rows))
                    .Group()
                    .Field("group")
                        .Input(false)
                        .Default("group")
                        .Aggregate("group")
                        .PrimaryKey();

            foreach (var field in result.Fields) {
                builder
                    .Field(field.Name)
                    .Length(field.Length)
                    .Type(field.Type)
                    .Aggregate(aggregate)
                    .Distinct(distinct);
            }

            return ProcessFactory.Create(builder.Process())[0].Execute()[aggregate].First();
        }

        private static void AddToProfile(ref Dictionary<string, Row> profile, Result result, string aggregate, bool distinct) {
            var minRow = GetBuilder(result, aggregate, distinct);
            foreach (var column in minRow.Columns.Where(c => !c.Equals("group"))) {
                profile[column][aggregate] = minRow[column];
            }
        }

        private bool IsValidFileName(string name) {
            var containsABadCharacter = new Regex("[" + Regex.Escape(string.Concat(Path.GetInvalidPathChars(), Path.GetInvalidFileNameChars())) + "]");
            if (containsABadCharacter.IsMatch(name)) {
                return false;
            };
            return true;
        }
    }
}
