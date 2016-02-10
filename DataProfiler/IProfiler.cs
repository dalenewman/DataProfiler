using System.Collections.Generic;

namespace DataProfiler {
    public interface IProfiler : IResolvable {
        IEnumerable<FieldProfile> Profile(ImportResult importResult, int displayLimit);
    }
}