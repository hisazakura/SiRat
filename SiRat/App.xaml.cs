using SiRat.Services.Converter;
using SiRat.Services.Document;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SiRat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new Thread(() => HtmlToPdf.InitializeDriver()).Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            HtmlToPdf.Dispose();
            TempDocuments.Dispose();
        }
    }
}
