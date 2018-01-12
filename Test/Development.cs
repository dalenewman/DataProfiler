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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataProfiler;
using DataProfiler.Autofac;
using NUnit.Framework;
using Transformalize.Configuration;

namespace Test {

    [TestFixture]
    public class Development {
        private const string Output = @"c:\Temp\Data\tempProfile.csv";

        [Test]
        public void TestExporter() {

            var file = Path.GetTempFileName();
            File.WriteAllText(file, @"t1,t2,t3,t4
Monday,10,1.1,1/1/2014
Tuesday,11,2.2,2/1/2014
Wednesday,12,3.3,3/1/2014
Wednesday,12,3.3,3/1/2014
Thursday,13,4.4,4/1/2014
Friday,14,5.5,5/1/2014
Saturday,15,6.6,6/1/2014");

            File.Delete(Output);

            var connection = new Connection { Name = "input", Provider = "file", File = file };
            using (var scope = new AutofacBootstrapper(connection)) {
                var result = scope.Resolve<IImporter>().Import(connection);
                var profile = scope.Resolve<IProfiler>().Profile(result, 30);
                var builder = new StringBuilder();
                builder.AppendLine("Name,Type,Position,Count,Min Value,Max Value,Min Length,Max Length");
                foreach (var field in profile) {
                    builder.AppendLine($"{field.Field.Name},{field.Field.Type},{field.Field.Index},{field.Count},\"{field.MinValue}\",\"{field.MaxValue}\",{field.MinLength},{field.MaxLength}");
                }
                File.WriteAllText(Output, builder.ToString());
            }

            Assert.IsTrue(File.Exists(Output));
        }

        [Test]
        public void TestProfiler() {
            File.WriteAllText(@"c:\Temp\Data\temp.txt", @"t1,t2,t3,t4
Monday,10,1.1,1/1/2014
Tuesday,11,2.2,2/1/2014
Wednesday,12,3.3,3/1/2014
Wednesday,12,3.3,3/1/2014
Thursday,13,4.4,4/1/2014
Friday,14,5.5,5/1/2014
Saturday,15,6.6,6/1/2014");

            var connection = new Connection {
                Name = "input",
                Provider = "file",
                File = @"c:\Temp\Data\temp.txt",
                Types = new List<TflType> {
                    new TflType("byte"),
                    new TflType("single"),
                    new TflType("datetime")
                }
            };

            using (var scope = new AutofacBootstrapper(connection)) {
                var result = scope.Resolve<IImporter>().Import(connection);
                var profile = scope.Resolve<IProfiler>().Profile(result, 30).ToList();

                Assert.AreEqual(",", result.Connection.Delimiter);
                Assert.AreEqual(4, result.Fields.Count);
                Assert.AreEqual(7, result.Rows.Count());

                var t1 = profile.First(fp => fp.Field.Name == "t1");
                var t2 = profile.First(fp => fp.Field.Name == "t2");
                var t3 = profile.First(fp => fp.Field.Name == "t3");
                var t4 = profile.First(fp => fp.Field.Name == "t4");

                Assert.AreEqual(4, profile.Count);

                Assert.AreEqual("string", t1.Field.Type);
                Assert.AreEqual("byte", t2.Field.Type);
                Assert.AreEqual("single", t3.Field.Type);
                Assert.AreEqual("datetime", t4.Field.Type);

                // account for system fields
                Assert.AreEqual(4, t1.Field.Index);
                Assert.AreEqual(5, t2.Field.Index);
                Assert.AreEqual(6, t3.Field.Index);
                Assert.AreEqual(7, t4.Field.Index);

                Assert.AreEqual("Friday", t1.MinValue);
                Assert.AreEqual(10, t2.MinValue);
                Assert.AreEqual(1.1f, t3.MinValue);
                Assert.AreEqual(new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Local), t4.MinValue);

                Assert.AreEqual("Wednesday", t1.MaxValue);
                Assert.AreEqual(15, t2.MaxValue);
                Assert.AreEqual(6.6f, t3.MaxValue);
                Assert.AreEqual(new DateTime(2014, 6, 1, 0, 0, 0, DateTimeKind.Local), t4.MaxValue);

                Assert.AreEqual(6, t1.MinLength);
                Assert.AreEqual(2, t2.MinLength);
                Assert.AreEqual(3, t3.MinLength);
                Assert.AreEqual(20, t4.MinLength);

                Assert.AreEqual(9, t1.MaxLength);
                Assert.AreEqual(2, t2.MaxLength);
                Assert.AreEqual(3, t3.MaxLength);
                Assert.AreEqual(20, t4.MaxLength);

                Assert.AreEqual(6, t1.Count);
                Assert.AreEqual(6, t2.Count);
                Assert.AreEqual(6, t3.Count);
                Assert.AreEqual(6, t4.Count);

            }


        }

        [Test]
        // [Ignore("Depends on NorthWind database on local SQL Server.")]
        public void TestProfilerDatabase() {

            var connection = new Connection { Name = "input", Provider = "sqlserver", Database="Northwind", Table = "Customers"};
            using (var scope = new AutofacBootstrapper(connection)) {
                var result = scope.Resolve<IImporter>().Import(connection);
                var profile = scope.Resolve<IProfiler>().Profile(result, 30);
                Assert.NotNull(result);
                Assert.NotNull(profile);
            }
        }

    }
}