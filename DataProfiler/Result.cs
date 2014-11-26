using System.Collections.Generic;
using Transformalize.Libs.Rhino.Etl;
using Transformalize.Main;

namespace DataProfiler {
    public class Result {

        private string _name = "Results";
        private Fields _fields = new Fields();
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        private string _provider = string.Empty;

        public Fields Fields {
            get { return _fields; }
            set { _fields = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public Dictionary<string, object> Properties {
            get { return _properties; }
            set { _properties = value; }
        }

        public string Provider {
            get { return _provider; }
            set { _provider = value; }
        }

        public IEnumerable<Row> Rows { get; set; }

    }
}