using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiRat.Model;
using SiRat.Model.Data;
using SiRat.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Services.Data.Tests
{
    [TestClass()]
    public class ReportDataLoaderTests
    {
        [TestMethod()]
        public void LoadFormatsTest()
        {
            List<FormatData> formats = ReportDataLoader.LoadFormats();
            Assert.AreEqual(3, formats.Count);
            Assert.AreEqual("[Identitas] $nama$", formats[0].FileNameWithoutExtension);
        }

        [TestMethod()]
        public void LoadTemplatesTest()
        {
            List<SpreadsheetData> templates = ReportDataLoader.LoadTemplates();
            Assert.AreEqual(3, templates.Count);
            Assert.AreEqual("[Identitas] $nama$", templates[0].FileNameWithoutExtension);
        }

        [TestMethod()]
        public void LoadSantriTest()
        {
            List<Santri> santriList = ReportDataLoader.LoadSantri();
            Assert.AreEqual(2, santriList.Count);
            Assert.AreEqual("Rehan Saputra", santriList[0].Name);
            Assert.AreEqual(3, santriList[0].Reports.Count);
            Assert.IsNotNull(santriList[0].Reports[0].FormatData);
            Assert.IsNotNull(santriList[0].Reports[0].TemplateData);
            Assert.AreEqual("[Identitas] Rehan Saputra", santriList[0].Reports[0].SpreadsheetData.FileNameWithoutExtension);
            Assert.AreEqual("[Identitas] $nama$", santriList[0].Reports[0].FormatData?.FileNameWithoutExtension);
            Assert.AreEqual("[Identitas] $nama$", santriList[0].Reports[0].TemplateData?.FileNameWithoutExtension);
        }
    }
}