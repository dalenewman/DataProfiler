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
using dp.autofac;
using System.Runtime.CompilerServices;
using Transformalize.Contracts;

namespace dp.cli {
   class Program {

      static void Main(string[] args) {

         Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options => {
               if (options.IsValid(out string errorMessage)) {
                  Run(options);
               } else {
                  System.Console.WriteLine($"Error: {errorMessage}");
                  Environment.Exit(1);
               }
            })
            .WithNotParsed(CommandLineError);
      }

      static void Run(Options options) {

         var connection = options.ToConnection();
         using (var scope = new AutofacBootstrapper(connection)) {
            var logger = scope.GetLogger();
            try {
               var result = scope.Resolve<IImporter>().Import(connection);
               if(result.Schema.Entities.Any() && result.Schema.Entities.First().Fields.Any()) {
                  var profile = scope.Resolve<IProfiler>().Profile(result, options.Limit);
                  var writer = scope.Resolve<IWriter>();
                  writer.Write(profile, Console.Out);
                  Environment.ExitCode = 0;
               } else {
                  foreach (var entry in logger.Log) {
                     Console.Error.WriteLine(entry.Message);
                  }
                  Environment.ExitCode = 1;
               }
            } catch (Exception ex) {
               Console.Error.WriteLine(ex.Message);
               foreach (var entry in logger.Log) {
                  Console.Error.WriteLine(entry.Message);
               }
               Environment.ExitCode = 1;
            }
         }
      }

      static void CommandLineError(IEnumerable<Error> errors) {
         Environment.Exit(1);
      }

   }
}
