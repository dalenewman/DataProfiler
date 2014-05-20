using System.Collections.Generic;
using Transformalize.Libs.Rhino.Etl;

namespace DataProfiler {
    public class Result {

        private List<Field> _fields = new List<Field>();
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        private string _provider = string.Empty;

        public List<Field> Fields {
            get { return _fields; }
            set { _fields = value; }
        }

        public Dictionary<string, object> Properties {
            get { return _properties; }
            set { _properties = value; }
        }

        public string Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public IEnumerable<Row> Rows { get; set; }

    }
}