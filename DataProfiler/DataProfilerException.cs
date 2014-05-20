using System;
using Transformalize.Libs.NLog;

namespace DataProfiler {
    public class DataProfilerException : Exception {
        private readonly Logger _log = LogManager.GetLogger("DataProfiler");
        public DataProfilerException(string format, params object[] args) {
            _log.Error(format, args);
        }
    }
}