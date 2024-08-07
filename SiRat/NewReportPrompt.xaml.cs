using SiRat.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SiRat
{
    /// <summary>
    /// Interaction logic for NewReportPrompt.xaml
    /// </summary>
    public partial class NewReportPrompt : Window
    {
        public NewReportPrompt()
        {
            InitializeComponent();
        }

        public class OnAddReportEventArgs : EventArgs
        {
            public SpreadsheetData SelectedTemplate { get; }

            public OnAddReportEventArgs(SpreadsheetData template)
            {
                SelectedTemplate = template;
            }
        }

        public delegate void OnAddReportEventHandler(object? sender, OnAddReportEventArgs e);
        public event OnAddReportEventHandler? OnAddReport;

        private void BuatRaporBaruButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            if (TemplateList.SelectedItem is SpreadsheetData spreadsheetData) OnAddReport?.Invoke(null, new(spreadsheetData));
        }
    }
}
