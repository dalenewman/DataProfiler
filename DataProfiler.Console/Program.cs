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
using System;
using DataProfiler.Autofac;

namespace DataProfiler.Console {
    class Program {

        static void Main(string[] args) {

            var options = new Options();
            if (args.Length > 0 && CommandLine.Parser.Default.ParseArguments(args, options)) {
                var connection = options.ToConnection();
                using (var scope = new AutofacBootstrapper(connection)) {
                    var result = scope.Resolve<IImporter>().Import(connection);
                    var profile = scope.Resolve<IProfiler>().Profile(result, options.Limit);
                    System.Console.Out.WriteLine("Name,Type,Position,Count,Min Value,Max Value,Min Length,Max Length");
                    foreach (var field in profile) {
                        System.Console.Out.WriteLine($"{field.Field.Name},{field.Field.Type},{field.Field.Index-3},{field.Count},\"{field.MinValue}\",\"{field.MaxValue}\",{field.MinLength},{field.MaxLength}");
                    }
                }
                Environment.ExitCode = 0;
            } else {
                System.Console.Error.WriteLine(options.GetUsage());
                Environment.ExitCode = 1;
            }

        }

    }
}
