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
using static SiRat.NewReportPrompt;

namespace SiRat
{
    /// <summary>
    /// Interaction logic for NewSantriPrompt.xaml
    /// </summary>
    public partial class NewSantriPrompt : Window
    {
        public NewSantriPrompt()
        {
            InitializeComponent();
        }

        public class OnAddSantriEventArgs : EventArgs
        {
            public string SantriName { get; }

            public OnAddSantriEventArgs(string name)
            {
                SantriName = name;
            }
        }

        public delegate void OnAddSantriEventHandler(object? sender, OnAddSantriEventArgs e);
        public event OnAddSantriEventHandler? OnAddSantri;

        private void TambahSantriButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            if (!SantriNameTextBox.Text.Equals("")) OnAddSantri?.Invoke(null, new(SantriNameTextBox.Text));
        }
    }
}
