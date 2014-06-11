using System.Collections.Generic;
using System.IO;
using System.Linq;
using Transformalize.Configuration.Builders;
using Transformalize.Libs.Rhino.Etl;
using Transformalize.Main;
using Transformalize.Main.Providers;

namespace DataProfiler {
    public class ProfileExporter {

        private readonly Dictionary<string, string> _autoProvider = new Dictionary<string, string> {
            {"htm", "html"},
            {"html", "html"},
            {"*", "file"}
        };

        public void Export(Dictionary<string, Row> profile, string file = null) {
            Process userDefined;
            AbstractConnection output;
            var input = new RowsOperation(profile.Select(kv => kv.Value));
            string provider = null;

            try {
                userDefined = ProcessFactory.Create("DataProfiler")[0];
                output = userDefined.Connections["output"];
            } catch {
                throw new DataProfilerException("You must define a DataProfiler process with an 'output' connection in the transformalize configuration section.");
            }

            // user may override output provider by changing file extension (e.g. .html, .csv, .txt)
            if (file != null) {
                var key = new FileInfo(file).Extension.Replace(".", string.Empty).ToLower();
                provider = _autoProvider.ContainsKey(key) ? _autoProvider[key] : _autoProvider["*"];
            }

            var builder = new ProcessBuilder("DataProfiler")
                .Connection("input")
                    .Provider("internal")
                .Connection("output")
                    .ConnectionString(output.GetConnectionString())
                    .Database(output.Database)
                    .DateFormat(output.DateFormat)
                    .Delimiter(output.Delimiter)
                    .ErrorMode(output.ErrorMode)
                    .File(file ?? output.File)
                    .Folder(output.Folder)
                    .IncludeFooter(output.IncludeFooter)
                    .IncludeHeader(output.IncludeHeader)
                    .Password(output.Password)
                    .Port(output.Port)
                    .Provider(provider ?? output.Type.ToString())
                    .SearchOption(output.SearchOption)
                    .SearchPattern(output.SearchPattern)
                    .Server(output.Server)
                    .Start(output.Start)
                    .User(output.User);

            foreach (var action in userDefined.Actions) {
                builder.Action(action.Action)
                    .After(action.After)
                    .Before(action.Before)
                    .Connection(action.Connection.Name)
                    .File(action.File)
                    .From(action.From)
                    .To(action.To)
                    .Url(action.Url)
                    .Modes(action.Modes.ToArray());
            }

            builder.Entity("ProfileExporter")
                .InputOperation(input)
                .Field("field").Label("Field").Length(128).PrimaryKey()
                .Field("type").Label("Type")
                .Field("index").Label("Index").Int32()
                .Field("count").Label("Distinct Count").Int64()
                .Field("min").Label("Min Value").Length(33)
                    .Transform("elipse").Length(30)
                .Field("max").Label("Max Value").Length(33)
                    .Transform("elipse").Length(30)
                .Field("minlength").Label("Min Length").Int64()
                .Field("maxlength").Label("Max Length").Int64();

            var process = ProcessFactory.Create(builder.Process())[0];
            process.ExecuteScaler();
        }
    }
}