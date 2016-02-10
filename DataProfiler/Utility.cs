using System.IO;
using System.Text.RegularExpressions;

namespace DataProfiler {
    public static class Utility {
        public static bool IsValidFileName(string name) {
            var containsABadCharacter = new Regex("[" + Regex.Escape(string.Concat(Path.GetInvalidPathChars(), Path.GetInvalidFileNameChars())) + "]");
            if (containsABadCharacter.IsMatch(name)) {
                return false;
            };
            return true;
        }
    }
}
