using System;
using System.Text.RegularExpressions;
using Transformalize.Libs.NLog;
using Transformalize.Main.Providers;

namespace DataProfiler {

    public class ConnectionModifier {

        private readonly Regex _escaper = new Regex(@"(?<=\[.*)\.(?=.*\])", RegexOptions.Compiled);
        private string _fullyQualifiedName;
        private string _schema = string.Empty;
        private string _name = string.Empty;
        private AbstractConnection _connection;
        private readonly Logger _logger = LogManager.GetLogger("Connection.Modifier");

        public string[] NameElements { get; set; }

        public string Schema {
            get { return _schema; }
            set { _schema = value; }
        }

        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        public string FullyQualifiedName {
            get { return _fullyQualifiedName; }
            set {
                _fullyQualifiedName = value;
                NameElements = ParseElements(value);
            }
        }

        public ConnectionModifier(string fullyQualifiedName) {
            FullyQualifiedName = fullyQualifiedName;
        }

        private string[] ParseElements(string name) {
            var resource = _escaper.Replace(name, "*");
            return resource.Split(".".ToCharArray(), StringSplitOptions.None);
        }

        public void Modify(ref AbstractConnection connection) {
            _connection = connection;
            if (connection.Schemas) {
                switch (NameElements.Length) {
                    case 4:
                        connection.Server = Clean(NameElements[0]);
                        connection.Database = Clean(NameElements[1]);
                        Schema = Clean(NameElements[2]);
                        Name = Clean(NameElements[3]);
                        break;
                    case 3:
                        connection.Database = Clean(NameElements[0]);
                        Schema = Clean(NameElements[1]);
                        Name = Clean(NameElements[2]);
                        break;
                    default:
                        throw new DataProfilerException("Can't parse {0}.  Expecting 3 to 4 elements delimited by dots (e.g. Server.Database.Schema.Table or Database.Schema.Table).", FullyQualifiedName);
                }
                _logger.Info("Connection modified, Server:{0}, Database:{1}, Schema:{2}, Name:{3}.", connection.Server, connection.Database, Schema, Name);
            } else {
                switch (NameElements.Length) {
                    case 3:
                        connection.Server = Clean(NameElements[0]);
                        connection.Database = Clean(NameElements[1]);
                        Name = Clean(NameElements[2]);
                        break;
                    case 2:
                        connection.Database = Clean(NameElements[0]);
                        Name = Clean(NameElements[1]);
                        break;
                    default:
                        throw new DataProfilerException("Can't parse {0}.  Expecting 2 to 3 elements delimited by dots (e.g. Server.Database.Table or Database.Table).", FullyQualifiedName);
                }
                _logger.Info("Connection modified, Server:{0}, Database:{1}, Name:{2}.", connection.Server, connection.Database, Name);
            }
        }

        private string Clean(string element) {
            return element
                .Replace('*', '.')
                .TrimStart(_connection.L.ToCharArray())
                .TrimEnd(_connection.R.ToCharArray());
        }

    }


}