using System.Globalization;

namespace DataProfiler {
    public class FileImporter {

        public Result Import(string file) {

            var temp = new JunkDrawer.FileImporter().Import(file);

            var result = new Result();
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