using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiRat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Utilities.Tests
{
    [TestClass()]
    public class IndonesianSpellingTests
    {
        [TestMethod()]
        public void IndonesianSpellingTest()
        {
            Assert.AreEqual("NOL", new IndonesianSpelling(0).ToString());
            Assert.AreEqual("SATU", new IndonesianSpelling(1).ToString());
            Assert.AreEqual("DUA", new IndonesianSpelling(2).ToString());
            Assert.AreEqual("TIGA", new IndonesianSpelling(3).ToString());
            Assert.AreEqual("EMPAT", new IndonesianSpelling(4).ToString());
            Assert.AreEqual("LIMA", new IndonesianSpelling(5).ToString());
            Assert.AreEqual("ENAM", new IndonesianSpelling(6).ToString());
            Assert.AreEqual("TUJUH", new IndonesianSpelling(7).ToString());
            Assert.AreEqual("DELAPAN", new IndonesianSpelling(8).ToString());
            Assert.AreEqual("SEMBILAN", new IndonesianSpelling(9).ToString());

            Assert.AreEqual("SEPULUH", new IndonesianSpelling(10).ToString());
            Assert.AreEqual("SEBELAS", new IndonesianSpelling(11).ToString());
            Assert.AreEqual("DUA BELAS", new IndonesianSpelling(12).ToString());

            Assert.AreEqual("DUA PULUH", new IndonesianSpelling(20).ToString());
            Assert.AreEqual("TIGA PULUH", new IndonesianSpelling(30).ToString());

            Assert.AreEqual("DUA PULUH SATU", new IndonesianSpelling(21).ToString());
            Assert.AreEqual("TIGA PULUH DUA", new IndonesianSpelling(32).ToString());

            Assert.AreEqual("SERATUS", new IndonesianSpelling(100).ToString());
            Assert.AreEqual("SERIBU", new IndonesianSpelling(1000).ToString());
        }
    }
}