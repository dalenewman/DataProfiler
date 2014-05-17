using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using JunkDrawer;

namespace Test {
    [TestFixture]
    public class Development {
        [Test]
        public void TestProfiler() {
            File.WriteAllText(@"temp.txt", @"t1,t2,t3,t4
Monday,10,1.1,1/1/2014
Tuesday,11,2.2,2/1/2014
Wednesday,12,3.3,3/1/2014
Thursday,13,4.4,4/1/2014
Friday,14,5.5,5/1/2014
Saturday,15,6.6,6/1/2014");

            var result = new FileImporter().Import(@"temp.txt");
            Assert.AreEqual(',', result.FileInformation.Delimiter);
            Assert.AreEqual(4, result.Fields.Count());
            Assert.AreEqual(6, result.Rows.Count());

            var profile = new DataProfiler.DataProfiler().Profile(result);

            Assert.AreEqual(4, profile.Count());

            Assert.AreEqual("string", profile["t1"]["type"]);
            Assert.AreEqual("byte", profile["t2"]["type"]);
            Assert.AreEqual("single", profile["t3"]["type"]);
            Assert.AreEqual("datetime", profile["t4"]["type"]);

            Assert.AreEqual("Friday", profile["t1"]["min"]);
            Assert.AreEqual(10, profile["t2"]["min"]);
            Assert.AreEqual(1.1f, profile["t3"]["min"]);
            Assert.AreEqual(new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Local), profile["t4"]["min"]);

            Assert.AreEqual("Wednesday", profile["t1"]["max"]);
            Assert.AreEqual(15, profile["t2"]["max"]);
            Assert.AreEqual(6.6f, profile["t3"]["max"]);
            Assert.AreEqual(new DateTime(2014, 6, 1, 0, 0, 0, DateTimeKind.Local), profile["t4"]["max"]);

            Assert.AreEqual(6, profile["t1"]["minlength"]);
            Assert.AreEqual(2, profile["t2"]["minlength"]);
            Assert.AreEqual(3, profile["t3"]["minlength"]);
            Assert.AreEqual(20, profile["t4"]["minlength"]);

            Assert.AreEqual(9, profile["t1"]["maxlength"]);
            Assert.AreEqual(2, profile["t2"]["maxlength"]);
            Assert.AreEqual(3, profile["t3"]["maxlength"]);
            Assert.AreEqual(20, profile["t4"]["maxlength"]);

        }

    }
}
