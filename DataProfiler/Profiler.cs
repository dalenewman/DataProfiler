#region license
// DataProfiler
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
using System.Collections.Generic;
using System.Linq;

namespace DataProfiler {
    public class Profiler : IProfiler {
        public IEnumerable<FieldProfile> Profile(ImportResult importResult, int displayLimit) {
            var memory = importResult.Rows.ToArray();
            return importResult.Fields.Where(f => !f.System).Select(f => new FieldProfile(displayLimit) {
                Field = f,
                Position = f.Ordinal,
                MinValue = memory.Min(r => f.Type == "byte[]" ? null : r[f]),
                MaxValue = memory.Max(r => f.Type == "byte[]" ? null : r[f]),
                MinLength = memory.Min(r => f.Type == "byte[]" ? 0 : r[f].ToString().Length),
                MaxLength = memory.Max(r => f.Type == "byte[]" ? 0 : r[f].ToString().Length),
                Count = memory.Select(r => f.Type == "byte[]" ? null : r[f].ToString()).Distinct().Count()
            }).AsParallel();
        }
    }
}
