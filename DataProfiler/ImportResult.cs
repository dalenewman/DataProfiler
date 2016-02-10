#region license
// DataProfiler
// Copyright 2013 Dale Newman
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Pipeline;
using Pipeline.Configuration;
using System.Collections.Generic;

namespace DataProfiler {
    public class ImportResult {
        public List<Field> Fields { get; set; } = new List<Field>();
        public IEnumerable<Row> Rows { get; set; }
        public Connection Connection { get; set; }
        public Schema Schema { get; set; }
    }
}