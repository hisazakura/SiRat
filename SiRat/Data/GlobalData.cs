using SiRat.Model;
using SiRat.Model.Data;
using SiRat.Services.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Data
{
    public class GlobalData
    {
        public static string DataDirectory = Path.Join(Environment.CurrentDirectory, "\\data");
        public static string FormatDirectory = Path.Join(Environment.CurrentDirectory, "\\format");
        public static string TemplateDirectory = Path.Join(Environment.CurrentDirectory, "\\template");

        public static ObservableCollection<Santri> SantriList = new();
        public static ObservableCollection<FormatData> FormatList = new();
        public static ObservableCollection<SpreadsheetData> Templatelist = new();
        private static Santri? _selectedSantri;
        public static Santri? SelectedSantri
        {
            get => _selectedSantri;
            set
            {
                _selectedSantri = value;
                SelectedSantriChanged?.Invoke(null, new());
            }
        }

        private static ReportData? _selectedReport;
        public static ReportData? SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                SelectedReportChanged?.Invoke(null, new());
            }
        }

        public static event EventHandler? SelectedSantriChanged;
        public static event EventHandler? SelectedReportChanged;

        public static void SetSantriList(IEnumerable<Santri> values)
        {
            SantriList.Clear();
            foreach(Santri santri in values)
            {
                SantriList.Add(santri);
            }
        }

        public static void SetFormatList(IEnumerable<FormatData> values)
        {
            FormatList.Clear();
            foreach (FormatData format in values)
            {
                FormatList.Add(format);
            }
        }

        public static void SetTemplatelist(IEnumerable<SpreadsheetData> values)
        {
            Templatelist.Clear();
            foreach (SpreadsheetData spreadsheet in values)
            {
                Templatelist.Add(spreadsheet);
            }
        }

        public static void LoadAll()
        {
            GlobalData.SetTemplatelist(ReportDataLoader.LoadTemplates());
            GlobalData.SetFormatList(ReportDataLoader.LoadFormats());
            GlobalData.SetSantriList(ReportDataLoader.LoadSantri(GlobalData.FormatList.ToList(), GlobalData.Templatelist.ToList()));
        }
    }
}
