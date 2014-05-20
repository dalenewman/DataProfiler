using System;
using DataProfiler;

namespace Console {

    class Program {

        private static string _input;
        private static string _output;

        static void Main(string[] args) {
            ValidateArguments(args);

            new ProfileExporter().Export(new Profiler().Profile(_input), _output);
        }

        private static void ValidateArguments(string[] args) {
            if (args.Length == 0) {
                System.Console.WriteLine("You must supply 1 or 2 arguments: 1. input file, 2. output file.");
                Environment.Exit(1);
            }

            if (args.Length > 2) {
                System.Console.WriteLine("DataProfiler only support 2 arguments: 1. input file, 2. output file.");
                Environment.Exit(1);
            }

            if (args.Length > 1) {
                _output = args[1];
            }

            _input = args[0];
        }
    }
}
