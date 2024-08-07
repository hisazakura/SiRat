using HtmlAgilityPack;
using SiRat.Data;
using SiRat.Model.Data;
using SiRat.Services.Converter;
using SiRat.Services.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Services.Document
{
    public class TempDocuments
    {
        private static List<string> _tempFiles = new();
        public static string? ApplyFormatAndSaveTemp(ReportData report)
        {
            if (report == null || report.FormatData == null || report.FormatData.ParentDirectory == null) return null;
            HtmlDocument document = ReportDataUtils.ApplyFormat(report.FormatData, report.SpreadsheetData);

            string targetPath = Path.Join(report.FormatData.ParentDirectory, "~$" + report.SpreadsheetData.FileNameWithoutExtension + ".html");
            document.Save(targetPath);

            _tempFiles.Add(targetPath);
            return targetPath;
        }

        public static async Task<string?> TempExport(string htmlPath)
        {
            if (HtmlToPdf.DriverState != HtmlToPdf.DriverStates.Initialized) return null;
            string tempPath = Path.ChangeExtension(htmlPath, ".pdf");
            await HtmlToPdf.ConvertHtmlToPdfAsync(htmlPath, tempPath);

            _tempFiles.Add(tempPath);
            return tempPath;
        }

        public static void Dispose()
        {
            foreach (string file in _tempFiles) File.Delete(file);
            _tempFiles.Clear();
        }
    }
}
