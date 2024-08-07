using SiRat.Data;
using SiRat.Model;
using SiRat.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Services.Data
{
    public class ReportDataLoader
    {
        private static string DataDirectory => GlobalData.DataDirectory;
        private static string FormatDirectory => GlobalData.FormatDirectory;
        private static string TemplateDirectory => GlobalData.TemplateDirectory;

        private static void EnsureFolderExists()
        {
            if (!Directory.Exists(DataDirectory)) Directory.CreateDirectory(DataDirectory);
            if (!Directory.Exists(FormatDirectory)) Directory.CreateDirectory(FormatDirectory);
            if (!Directory.Exists(TemplateDirectory)) Directory.CreateDirectory(TemplateDirectory);
        }

        public static List<FormatData> LoadFormats()
        {
            EnsureFolderExists();
            return Directory.GetFiles(FormatDirectory, "*.html").Where(file => !file.Contains("~$")).Select(file => FormatData.FromFormat(file)).ToList();
        }

        public static List<SpreadsheetData> LoadTemplates()
        {
            EnsureFolderExists();
            return Directory.GetFiles(TemplateDirectory, "*.xlsx").Where(file => !file.Contains("~$")).Select(file => SpreadsheetData.FromSpreadsheet(file)).ToList();
        }

        public static List<Santri> LoadSantri()
        {
            return LoadSantri(LoadFormats(), LoadTemplates());
        }

        public static List<Santri> LoadSantri(List<FormatData> loadedFormats, List<SpreadsheetData> loadedTemplates)
        {
            EnsureFolderExists();
            List<Santri> data = new();
            foreach (string directory in Directory.GetDirectories(DataDirectory))
            {
                string name = new DirectoryInfo(directory).Name;
                Santri santri = new(name);
                List<ReportData> reports = new();
                foreach (string file in Directory.GetFiles(directory, "*.xlsx"))
                {
                    if (file.Contains("~$")) continue;
                    SpreadsheetData spreadsheet = SpreadsheetData.FromSpreadsheet(file);

                    string targetName = spreadsheet.FileNameWithoutExtension;
                    FormatData? format = loadedFormats.FirstOrDefault(format => format.FileNameWithoutExtension.Replace("$nama$", name).Equals(targetName));
                    SpreadsheetData? template = loadedTemplates.FirstOrDefault(template => template.FileNameWithoutExtension.Replace("$nama$", name).Equals(targetName));

                    ReportData report = new(spreadsheet) { Parent = santri };
                    if (format != null) report.FormatData = format;
                    if (template != null) report.TemplateData = template;

                    reports.Add(report);
                }
                santri.Reports = reports;
                data.Add(santri);
            }
            return data;
        }
    }
}
