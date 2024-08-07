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
    public class SpreadsheetDataTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            GlobalData.LoadAll();
        }

        [TestMethod()]
        public void GetParentTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            Assert.AreEqual(GlobalData.SantriList[0].Reports[0], GlobalData.SantriList[0].Reports[0].SpreadsheetData.GetParent());
        }

        [TestMethod()]
        public void QueryTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("\"Data Santri\".\"Nama\"[0]");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is string);
            Assert.AreEqual("Rehan Saputra", (string)result);
        }

        [TestMethod()]
        public void SumAcrossTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("SUMACROSS(\"Muatan Unggulan 1\".\"Nilai\"[0])");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is double);
            Assert.AreEqual(164.0, (double)result);
        }

        [TestMethod()]
        public void AvgAcrossTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("AVGACROSS(\"Muatan Unggulan 1\".\"Nilai\"[0])");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is double);
            Assert.AreEqual(82.0, (double)result);
        }

        [TestMethod()]
        public void SpellTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("SPELL(\"Muatan Unggulan 1\".\"Nilai\"[0])");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is string);
            Assert.AreEqual("Delapan Puluh Satu", (string)result);
        }

        [TestMethod()]
        public void EqTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("EQ(\"Target\".\"Keterangan\"[0],✔,True)");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is string);
            Assert.AreEqual("True", (string)result);
        }

        [TestMethod()]
        public void GradeTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("GRADE(\"Doa\".\"Nilai\"[0])");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is string);
            Assert.AreEqual("A", (string)result);
        }

        [TestMethod()]
        public void ReverseTest()
        {
            Assert.IsTrue(GlobalData.SantriList.Count > 0);
            Assert.IsTrue(GlobalData.SantriList[0].Reports.Count > 0);
            ReportData? report = GlobalData.SantriList[0].Reports.FirstOrDefault(report => report.TemplateData?.FileNameWithoutExtension.Equals("[KLS A S1] $nama$") == true);
            Assert.IsNotNull(report);

            object? result = report.SpreadsheetData.Query("REVERSE(\"Muatan Unggulan 2\".\"Capaian\"[0])");
            Assert.IsNotNull(result);
            Assert.IsTrue(result is string);
            Assert.AreEqual(string.Join("", "٢٤ - ١ ص".Reverse()), (string)result);
        }
    }
}