using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiRat.Data;
using SiRat.Model.Data;
using SiRat.Services.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Services.Document.Tests
{
    [TestClass()]
    public class TempDocumentsTests
    {
        [TestInitialize()]
        public void Initialize()
        {
            GlobalData.LoadAll();
        }

        [TestMethod()]
        public void ApplyFormatAndSaveTempTest()
        {
            ReportData? report = GlobalData.SantriList.FirstOrDefault(santri => santri.Name.Equals("Rehan Saputra"))?.Reports.FirstOrDefault(r => r.FormatData?.FileNameWithoutExtension.Equals("[Identitas] $nama$") == true);
            Assert.IsNotNull(report);
            Assert.IsNotNull(report.FormatData);

            string? tempPath = TempDocuments.ApplyFormatAndSaveTemp(report);
            Assert.IsNotNull(tempPath);
            Assert.IsTrue(File.Exists(tempPath));

            TempDocuments.Dispose();
            Assert.IsFalse(File.Exists(tempPath));
        }
    }
}