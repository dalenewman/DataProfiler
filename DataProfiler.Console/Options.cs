#region license
// DataProfiler.Console
// Copyright 2013 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.Collections.Generic;
using Cfg.Net.Ext;
using CommandLine;
using CommandLine.Text;
using Pipeline.Configuration;

namespace DataProfiler.Console {


    public class Options {

        [Option('s', "server", Required = false, DefaultValue = "localhost", HelpText = "The server's name or ip address.")]
        public string Server { get; set; }

        [Option('d', "database", Required = false, DefaultValue = "", HelpText = "The database name.")]
        public string Database { get; set; }

        [Option('o', "schema owner", Required = false, DefaultValue = "", HelpText = "The schema or owner name.")]
        public string Schema { get; set; }

        [Option('t', "table", Required = false, DefaultValue = "", HelpText = "The table (or view) name.")]
        public string Table { get; set; }

        [Option('u', "user", Required = false, DefaultValue = "", HelpText = "The user name.")]
        public string User { get; set; }

        [Option('p', "password", Required = false, DefaultValue = "", HelpText = "The password.")]
        public string Password { get; set; }

        [Option('f', "file", Required = false, DefaultValue = "", HelpText = "The file.")]
        public string File { get; set; }

        [Option('c', "connection type", Required = false, DefaultValue = "sqlserver", HelpText = "The connection type or provider (i.e. sqlserver, mysql, postgresql, sqlite, file, or excel.)")]
        public string Provider { get; set; }

        [Option('n', "port number", DefaultValue = 0, HelpText = "")]
        public int Port { get; set; }

        [Option('l', "limit", DefaultValue = 15, HelpText = "To limit the Min Value and Max Value text returned.")]
        public int Limit { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public Connection ToConnection() {
            return new Connection {
                Name = "input",
                Key = "input",
                Provider = File == string.Empty ? Provider : "file",
                Server = Server,
                Database = Database,
                Schema = Schema,
                Table = Table,
                File = File,
                Port = Port,
                User = User,
                Password = Password,
                Types = new List<TflType> {
                    new TflType("bool"),
                    new TflType("byte"),
                    new TflType("short"),
                    new TflType("int"),
                    new TflType("long"),
                    new TflType("single"),
                    new TflType("double"),
                    new TflType("decimal"),
                    new TflType("datetime")
                }
            }.WithDefaults();
        }

    }
}