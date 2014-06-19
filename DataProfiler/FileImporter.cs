using System.Globalization;
using System.IO;
using JunkDrawer;
using Transformalize.Main.Providers.File;

namespace DataProfiler {
    public class FileImporterWrapper {

        public Result Import(string file, decimal sample = 100m) {
            var result = new Result();
            var fileInfo = new FileInfo(file);
            var connection = JunkDrawerConfiguration.GetTransformalizeConnection();
            var request = JunkDrawerConfiguration.GetFileInspectionRequest();
            request.Sample = sample > 0m || sample < 100m ? sample : request.Sample;

            var temp = new FileImporter().Import(fileInfo, request, connection);

            foreach (var field in temp.Information.Fields) {
                result.Fields.Add(new Field(field.Name, field.Type, field.Length));
            }

            result.Rows = temp.Rows;
            result.Provider = "file";
            result.Properties["filename"] = temp.Information.FileInfo.FullName;
            result.Properties["delimiter"] = temp.Information.Delimiter.ToString(CultureInfo.InvariantCulture);
            return result;
        }
    }
}