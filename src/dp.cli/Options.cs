#region license
// Data Profiler
// Copyright © 2013-2025 Dale Newman
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
using CommandLine;
using Transformalize.Configuration;

namespace dp.cli {


   [Verb("profile", isDefault: true, HelpText = "Run Data Profiler")]
   public class Options {

      [Option('c', "connection type", Required = true, Default = "", HelpText = "The connection type or provider (i.e. sqlserver, mysql, postgresql, sqlite, file, or excel.)")]
      public string Provider { get; set; }

      [Option('s', "server", Required = false, Default = "", HelpText = "The server's name or ip address.")]
      public string Server { get; set; }

      [Option('d', "database", Required = false, Default = "", HelpText = "The database name.")]
      public string Database { get; set; }

      [Option('o', "schema", Required = false, Default = "", HelpText = "The schema name.")]
      public string Schema { get; set; }

      [Option('t', "table", Required = false, Default = "", HelpText = "The table (or view) name.")]
      public string Table { get; set; }

      [Option('u', "user", Required = false, Default = "", HelpText = "The user name.")]
      public string User { get; set; }

      [Option('p', "password", Required = false, Default = "", HelpText = "The password.")]
      public string Password { get; set; }

      [Option('f', "file", Required = false, Default = "", HelpText = "The file.")]
      public string File { get; set; }

      [Option('n', "port number", Default = 0, HelpText = "")]
      public int Port { get; set; }

      [Option('l', "limit", Default = 15, HelpText = "To limit the Min Value and Max Value text returned.")]
      public int Limit { get; set; }

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
         };
      }

      public bool IsValid(out string errorMessage) {

         if (!string.IsNullOrWhiteSpace(Provider)) {

            if (Provider.Equals("file", StringComparison.CurrentCultureIgnoreCase) || Provider.Equals("sqlite", StringComparison.CurrentCultureIgnoreCase) || Provider.Equals("excel", StringComparison.CurrentCultureIgnoreCase)) {
               if (string.IsNullOrWhiteSpace(File) || string.IsNullOrEmpty(File)) {
                  errorMessage = "When connection type is file, excel, or sqlite, you must also provide a file argument.";
                  return false;
               } else {
                  errorMessage = string.Empty;
                  return true;
               }
            }

            if (!string.IsNullOrWhiteSpace(Server) && !string.IsNullOrEmpty(Server) &&
                !string.IsNullOrWhiteSpace(Database) && !string.IsNullOrEmpty(Database) &&
                !string.IsNullOrWhiteSpace(Table) && !string.IsNullOrEmpty(Table)) {
               errorMessage = string.Empty;
               return true;
            }
         }

         // If neither condition is met, return an error
         errorMessage = "You must provide a connection type and file or a combination of server, database, and table.";
         return false;
      }


   }
}