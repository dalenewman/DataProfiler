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
using System;
using System.Collections.Generic;
using System.Linq;

namespace dp {
    public class InMemoryProfiler : IProfiler {
        public IList<FieldProfile> Profile(ImportResult importResult, int displayLimit) {
            var memory = importResult.Rows.ToArray();
            return importResult.Fields.Where(f => !f.System && f.Type != "byte[]").Select(f => new FieldProfile(displayLimit) {
                Field = f,
                Position = f.Ordinal,
                MinValue = memory.Min(r => r[f]),
                MaxValue = memory.Max(r => r[f]),
                MinLength = memory.Min(r => r[f].ToString().Length),
                MaxLength = memory.Max(r => r[f].ToString().Length),
                Count = memory.Select(r => r[f].ToString()).Distinct().Count()
            }).AsParallel().ToList();
        }
    }
}
