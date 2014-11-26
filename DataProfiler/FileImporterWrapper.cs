using System.Globalization;
using System.IO;
using Transformalize.Configuration;
using Transformalize.Main;
using Transformalize.Main.Providers.File;

namespace DataProfiler {
    public class FileImporterWrapper : IImporter {

        public Result Import(string file, decimal sample = 100m) {

            var fileInspection = new ConfigurationFactory("DataProfiler").CreateSingle().FileInspection.GetInspectionRequest();
            
            fileInspection.Sample = sample > 0m || sample < 100m ? sample : fileInspection.Sample;

            var response = new FileImporter().Import(
                new FileInfo(file),
                fileInspection,
                new ConnectionConfigurationElement() { Name = "output", Provider = "internal" }
            );

            var result = new Result {
                Fields = response.Information.Fields,
                Rows = response.Rows,
                Provider = "file"
            };
            result.Properties["filename"] = response.Information.FileInfo.FullName;
            result.Properties["delimiter"] = response.Information.Delimiter.ToString(CultureInfo.InvariantCulture);
            return result;
        }
    }
}