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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace dp {
   public class LessMemoryProfiler : IProfiler {
      public IList<FieldProfile> Profile(ImportResult importResult, int displayLimit) {

         var profiles = new List<FieldProfile>();
         foreach (var field in importResult.Fields.Where(f => !f.System)) {
            profiles.Add(new FieldProfile(displayLimit) { Field = field, Position = field.Ordinal });
         }

         foreach (var row in importResult.Rows) {

            foreach (var fp in profiles) {

               if (fp.Field.Type != "byte[]") {
                  var value = row[fp.Field];
                  var valueString = value?.ToString(); // Handle null value safely

                  // Ensure MinValue is initialized and handle nulls
                  if (fp.MinValue == null || (value != null && Comparer<object>.Default.Compare(fp.Field.Convert(value), fp.MinValue) < 0)) {
                     fp.MinValue = value;
                  }

                  // Update MaxValue
                  if (fp.MaxValue == null || (value != null && Comparer<object>.Default.Compare(fp.Field.Convert(value), fp.MaxValue) > 0)) {
                     fp.MaxValue = value;
                  }

                  // Update MinLength and MaxLength
                  if (valueString != null) {
                     fp.MinLength = fp.Started ? Math.Min(fp.MinLength, valueString.Length) : valueString.Length;
                     fp.MaxLength = Math.Max(fp.MaxLength, valueString.Length);
                     fp.DistinctValues.Add(valueString);
                  }
               }
               fp.Started = true;
            }

         }

         return profiles;
      }
   }
}
