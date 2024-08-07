using SiRat.Data;
using SiRat.Model.Data;
using SiRat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace SiRat.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedReportName;
        public ObservableCollection<Santri> SantriList => GlobalData.SantriList;
        public ObservableCollection<FormatData> FormatList => GlobalData.FormatList;
        public ObservableCollection<SpreadsheetData> TemplateList => GlobalData.Templatelist;
        public Santri? SelectedSantri => GlobalData.SelectedSantri;
        public ReportData? SelectedReport => GlobalData.SelectedReport;
        public string SelectedReportName
        {
            get => _selectedReportName;
            private set
            {
                if (_selectedReportName == value) return;

                _selectedReportName = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FileInfoData> FileInfoDataList { get; } = new();

        public MainViewModel()
        {
            _selectedReportName = GlobalData.SelectedReport?.SpreadsheetData.FileNameWithoutExtension ?? "Pilih Rapor";
            GlobalData.SelectedReportChanged += (object? sender, EventArgs e) =>
            {
                SelectedReportName = GlobalData.SelectedReport?.SpreadsheetData.FileNameWithoutExtension ?? "Pilih Rapor";
                UpdateFileInfoDataList();
            };
        }

        private void UpdateFileInfoDataList()
        {
            FileInfoDataList.Clear();
            if (SelectedReport == null) return;
            FileInfoDataList.Add(new FileInfoData("Lokasi File", ":", SelectedReport?.SpreadsheetData.FullPath ?? ""));
            FileInfoDataList.Add(new FileInfoData("Lokasi Format", ":", SelectedReport?.FormatData?.FullPath ?? ""));
            FileInfoDataList.Add(new FileInfoData("Lokasi Template", ":", SelectedReport?.TemplateData?.FullPath ?? ""));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Additional logic for ViewModel can be added here
    }
}
