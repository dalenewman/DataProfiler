using System.Collections.Generic;
using System.Linq;
using JunkDrawer;
using Transformalize.Configuration.Builders;
using Transformalize.Libs.Rhino.Etl;
using Transformalize.Main;

namespace DataProfiler {
    public class DataProfiler {

        public Dictionary<string, Row> Profile(Result result) {

            var profile = result.Fields.ToDictionary(field => field.Name, field => new Row() { { "type", field.Type } });

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

        private static Row GetBuilder(Result result, string aggregate, bool distinct) {
            var builder = new ProcessBuilder(result.FileInformation.ProcessName)
                .Star(result.FileInformation.ProcessName)
                .Connection("input")
                    .Provider("internal")
                .Connection("output")
                    .Provider("internal")
                .Entity("profile")
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

            return ProcessFactory.Create(builder.Process())[0].Run()["profile"].First();
        }

        private static void AddToProfile(ref Dictionary<string, Row> profile, Result result, string aggregate, bool distinct) {
            var minRow = GetBuilder(result, aggregate, distinct);
            foreach (var column in minRow.Columns.Where(c => !c.Equals("group"))) {
                profile[column][aggregate] = minRow[column];
            }
        }
    }
}
