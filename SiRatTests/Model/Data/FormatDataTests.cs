using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiRat.Data;
using SiRat.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Model.Data.Tests
{
    [TestClass()]
    public class FormatDataTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            GlobalData.LoadAll();
        }

        [TestMethod()]
        public void GetMetaTest()
        {
            FormatData? format = GlobalData.FormatList.FirstOrDefault(f => f.FileNameWithoutExtension.Equals("[KLS A S1] $nama$"));
            Assert.IsNotNull(format);

            string? meta = format.GetMeta("doc-size");
            Assert.IsNotNull(meta);
            Assert.AreEqual("f4", meta);
        }
    }
}