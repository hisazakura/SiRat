using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiRat.Data;
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
    public class ReportDataUtilsTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            GlobalData.LoadAll();
        }

        [TestMethod()]
        public void ApplyFormatTest()
        {
            ReportData? report = GlobalData.SantriList.FirstOrDefault(santri => santri.Name.Equals("Rehan Saputra"))?.Reports.FirstOrDefault(r => r.FormatData?.FileNameWithoutExtension.Equals("[Identitas] $nama$") == true);
            Assert.IsNotNull(report);
            Assert.IsNotNull(report.FormatData);

            HtmlDocument expected = new();
            expected.Load(Path.Join(GlobalData.FormatDirectory, "~$[Identitas] Rehan Saputra - Test.html"));
            ReportDataUtils.ApplyFormat(report.FormatData, report.SpreadsheetData).Save(Path.Join(GlobalData.FormatDirectory, "~$[Identitas] Rehan Saputra - Test 2.html"));

            HtmlDocument actual = new();
            actual.Load(Path.Join(GlobalData.FormatDirectory, "~$[Identitas] Rehan Saputra - Test 2.html"));

            Assert.AreEqual(expected.Text, actual.Text);
        }
    }
}