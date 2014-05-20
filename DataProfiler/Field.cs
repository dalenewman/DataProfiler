namespace DataProfiler {
    public class Field {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Length { get; set; }

        public Field(string name, string type, string length) {
            Name = name;
            Type = type;
            Length = length;
        }
    }
}