using System.Globalization;

namespace DataProfiler {
    public class FileImporter {

        public Result Import(string file, decimal sample = 100m) {

            var temp = new JunkDrawer.FileImporter().Import(file, sample);

            var result = new Result {
                Name = temp.FileInformation.FileInfo.Name.Replace(temp.FileInformation.FileInfo.Extension, string.Empty)
            };
            foreach (var field in temp.Fields) {
                result.Fields.Add(new Field(field.Name, field.Type, field.Length));
            }

            result.Rows = temp.Rows;
            result.Provider = "file";
            result.Properties["filename"] = temp.FileInformation.FileInfo.FullName;
            result.Properties["delimiter"] = temp.FileInformation.Delimiter.ToString(CultureInfo.InvariantCulture);
            return result;
        }
    }
}