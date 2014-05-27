using System;
using System.Collections.Generic;
using DataProfiler;

namespace Console {

    class Program {

        private static string _input;
        private static string _output;
        private static decimal _sample = 100m;

        static void Main(string[] args) {
            ValidateArguments(args);

            new ProfileExporter().Export(new Profiler().Profile(_input, _sample), _output);
        }

        private static void ValidateArguments(IList<string> args) {
            if (args.Count == 0) {
                PrintUsage();
                Environment.Exit(1);
            }

            if (args.Count > 3) {
                PrintUsage();
                Environment.Exit(1);
            }

            if (args.Count > 1) {
                _output = args[1];
            }

            if (args.Count > 2) {
                int sample;
                if (int.TryParse(args[2], out sample)) {
                    if (sample > 0 && sample <= 100) {
                        _sample = sample;
                    }
                } else {
                    System.Console.WriteLine("Error parson 3rd argument.");
                    PrintUsage();
                    Environment.Exit(1);
                }

            }

            _input = args[0];
        }

        private static void PrintUsage() {
            System.Console.WriteLine("You must supply 1 to 3 valid arguments.");
            System.Console.WriteLine("1: input file or fully qualified table");
            System.Console.WriteLine("2: output file.");
            System.Console.WriteLine("3: sample percentage. A value between 1 and 100 (defaults to 100 percent).");
        }
    }
}
