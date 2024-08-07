using HtmlAgilityPack;
using SiRat.Data;
using SiRat.Model;
using SiRat.Model.Data;
using SiRat.Services.Converter;
using SiRat.Services.Data;
using SiRat.Services.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shell;

namespace SiRat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool StopPromptDownload = false;
        private bool StopPromptReload = false;
        public MainWindow()
        {
            InitializeComponent();
            GlobalData.SelectedReportChanged += OnSelectedReportChanged;
            GlobalData.SelectedSantriChanged += OnSelectedSantriChanged;
        }

        private void PromptToDownloadChrome()
        {
            if (StopPromptDownload) return;
            Popup popup = new("Error", "ChromeDriver tidak dapat diaktifkan, pastikan Anda memiliki aplikasi Google Chrome atau lakukan konversi manual.");
            popup.AddButton("Panduan Konversi Manual");
            popup.AddButton("Download Google Chrome").Click += (object? sender, RoutedEventArgs e) => {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "https://www.google.com/chrome/browser-tools/",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"Failed to open URL: {ex.Message}");
                }
            };

            CheckBox stopPrompt = new() { Content = "Jangan tanya lagi", Margin = new Thickness(16, 0, 16, 0) };
            DockPanel.SetDock(stopPrompt, Dock.Bottom);
            popup.GetContentContainer().Children.Insert(0, stopPrompt);
            popup.Closing += (object? sender, System.ComponentModel.CancelEventArgs e) =>
            {
                StopPromptDownload = stopPrompt.IsChecked == true;
            };
            popup.Show();
        }

        private void ChromeIsInitializingPopup()
        {
            Popup popup = new("ChromeDriver sedang diinisialisasi. Mohon tunggu dan coba lagi.");
            popup.Show();
        }

        private void ChromeCrashedPopup()
        {
            if (StopPromptReload) return;
            Popup popup = new("Terdapat masalah pada ChromeDriver, mohon tutup dan buka kembali aplikasi ini untuk melanjutkan.");
            popup.AddButton("Lanjutkan tanpa ChromeDriver").Click += (object? sender, RoutedEventArgs e) => {
                StopPromptReload = true;
            };
            popup.Show();
        }

        private async void OnSelectedReportChanged(object? sender, EventArgs e)
        {
            if (GlobalData.SelectedReport == null)
            {
                OpenFileButton.IsEnabled = false;
                ExportFileButton.IsEnabled = false;
                PreviewBrowser.Navigate(new Uri("about:blank"));
                return;
            }
            OpenFileButton.IsEnabled = true;
            ExportFileButton.IsEnabled = true;
            string? htmlPath = TempDocuments.ApplyFormatAndSaveTemp(GlobalData.SelectedReport);
            if (htmlPath == null)
            {
                PreviewBrowser.Navigate(new Uri("about:blank"));
                return;
            }
            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.Crashed) { ChromeCrashedPopup(); return; }
            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.Initializing) { ChromeIsInitializingPopup(); return; }
            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.NotInitialized) { PromptToDownloadChrome(); return; }
            string? pdfPath = await TempDocuments.TempExport(htmlPath);
            if (pdfPath == null)
            {
                PreviewBrowser.Navigate(new Uri("about:blank"));
                return;
            }

            PreviewBrowser.Navigate($"file:///{pdfPath.Replace(" ", "%20")}");
        }

        private void OnSelectedSantriChanged(object? sender, EventArgs e)
        {
            if (GlobalData.SelectedSantri == null) { BuatRaporBaruButton.IsEnabled = false; return; }
            BuatRaporBaruButton.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PreviewBrowser.Navigate(new Uri("about:blank"));
            GlobalData.LoadAll();
        }

        private void SantriTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            GlobalData.SelectedReport = (SantriTreeView.SelectedItem is ReportData selectedReport) ? selectedReport : null;
            GlobalData.SelectedSantri = (SantriTreeView.SelectedItem is Santri selectedSantri) ? selectedSantri : null;
        }

        private async void ExportFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.SelectedReport == null) return; 
            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.Crashed) { ChromeCrashedPopup(); return; }
            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.Initializing) { ChromeIsInitializingPopup(); return; }

            string? htmlPath = TempDocuments.ApplyFormatAndSaveTemp(GlobalData.SelectedReport);
            if (htmlPath == null) return;

            if (HtmlToPdf.DriverState == HtmlToPdf.DriverStates.NotInitialized) { PromptToDownloadChrome(); return; }
            string pdfPath = Path.ChangeExtension(GlobalData.SelectedReport.SpreadsheetData.FullPath, ".pdf");
            await HtmlToPdf.ConvertHtmlToPdfAsync(htmlPath, pdfPath);
            ProcessStartInfo processStartInfo = new() { FileName = pdfPath, UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        private void BuatRaporBaruButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.SelectedSantri == null) return;
            Santri santri = GlobalData.SelectedSantri;
            NewReportPrompt prompt = new();
            prompt.OnAddReport += (object? sender, NewReportPrompt.OnAddReportEventArgs e) =>
            {
                SpreadsheetData selected = e.SelectedTemplate;
                foreach (ReportData report in santri.Reports) if (report.SpreadsheetData.FileNameWithoutExtension == selected.FileNameWithoutExtension.Replace("$nama$", santri.Name)) return;
                File.Copy(selected.FullPath, Path.Join(GlobalData.DataDirectory, santri.Name, selected.FileName.Replace("$nama$", santri.Name)));
                GlobalData.LoadAll();
            };
            prompt.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PreviewBrowser.Navigate("about:blank");
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F5) return;
            PreviewBrowser.Navigate("about:blank");
            GlobalData.LoadAll();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalData.SelectedReport == null) return;
            ProcessStartInfo processStartInfo = new() { FileName = GlobalData.SelectedReport.SpreadsheetData.FullPath, UseShellExecute = true };
            Process.Start(processStartInfo);
        }

        private void TambahSantriBaruButton_Click(object sender, RoutedEventArgs e)
        {
            NewSantriPrompt prompt = new();
            prompt.OnAddSantri += (object? sender, NewSantriPrompt.OnAddSantriEventArgs e) =>
            {
                string santriPath = Path.Join(GlobalData.DataDirectory, e.SantriName);
                if (Directory.Exists(santriPath))
                {
                    new Popup("Error", "Santri dengan nama tersebut sudah ada.").Show();
                    return;
                }

                Directory.CreateDirectory(santriPath);
                PreviewBrowser.Navigate("about:blank");
                GlobalData.LoadAll();
            };
            prompt.Show();
        }
    }
}
