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
using Transformalize.Configuration;
using Transformalize.Extensions;

namespace dp {

   public class FieldProfile {

      private readonly int _displayLimit;
      private object _minValue;
      private object _maxValue;
      private HashSet<string> _distinctValues;
      private int count;

      public FieldProfile(int displayLimit) {
         _displayLimit = displayLimit;
         _distinctValues = new HashSet<string>();
         Started = false;
      }
      public Field Field { get; set; }

      public object MinValue {
         get => _minValue ?? null;
         set {
            if (value != null) {
               if (_displayLimit < 36 && value is Guid || value is string && value.ToString().Length > _displayLimit) {
                  value = value.ToString().Left(_displayLimit) + "...";
               }
               _minValue = value;
            }

         }
      }

      public object MaxValue {
         get => _maxValue ?? null;
         set {
            if (value != null) {
               if (_displayLimit < 36 && value is Guid || value is string && value.ToString().Length > _displayLimit) {
                  value = value.ToString().Left(_displayLimit) + "...";
               }
               _maxValue = value;
            }
         }
      }

      public int MinLength { get; set; }
      public int MaxLength { get; set; }
      public int Count { get => count == 0 ? _distinctValues.Count : count ; set => count = value; }
      public int Position { get; set; }

      public bool Started { get; set; }

      public HashSet<string> DistinctValues { 
         get {
            return _distinctValues;
         } 
         set {
            _distinctValues = value;
         }
      }
   }
}