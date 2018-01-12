#region license
// Data Profiler
// Copyright © 2013-2018 Dale Newman
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
using Transformalize.Configuration;
using Transformalize.Contracts;

namespace DataProfiler {

    class FieldRow {

        private short _isComparable = 0;
        public Field Field { get; }
        public IRow Row { get; }

        public FieldRow(Field f, IRow r) {
            Field = f;
            Row = r;
        }

        public bool IsComparable() {
            if (_isComparable == 0) {
                _isComparable = Row[Field] is IComparable ? (short)1 : (short)-1;
            }
            return _isComparable > 0;
        }
    }
}