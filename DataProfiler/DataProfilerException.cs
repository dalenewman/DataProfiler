using System;
using Transformalize.Logging;

namespace DataProfiler {
    public class DataProfilerException : Exception {
        public DataProfilerException(string format, params object[] args) {
            TflLogger.Error("DataProfiler","", format, args);
        }
    }
}