using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using System.IO;
using System.Threading;
using HtmlAgilityPack;
using System.Xml.Linq;

namespace SiRat.Services.Converter
{
    public class HtmlToPdf
    {
        private static bool _initializing = false;
        private static bool _crashed = false;
        public static DriverStates DriverState
        {
            get
            {
                return _initializing ? DriverStates.Initializing : _driver == null ? DriverStates.NotInitialized : _crashed ? DriverStates.Crashed : DriverStates.Initialized;
            }
        }
        private static ChromeDriver? _driver;
        private static readonly object Lock = new();

        public enum DriverStates
        {
            NotInitialized,
            Initializing,
            Initialized,
            Crashed
        }

        public static void InitializeDriver()
        {
            _initializing = true;

            try
            {
                new DriverManager().SetUpDriver(new ChromeConfig());
            }
            catch (Exception ex)
            {
                _initializing = false;
                Trace.TraceError($"Error during ChromeDriver setup: {ex.Message}");
            }
            try
            {
                lock (Lock)
                {
                    if (_driver != null) return;
                    _driver = new ChromeDriver(GetChromeDriverService(), GetChromeOptions());
                }
            }
            catch (Exception ex)
            {
                _initializing = false;
                Trace.TraceError($"Error during ChromeDriver setup: {ex.Message}");
            }
            _initializing = false;

        }

        private static ChromeOptions GetChromeOptions()
        {
            ChromeOptions chromeOptions = new();
            chromeOptions.AddArguments("--headless", "--disable-gpu", "--no-sandbox");
            chromeOptions.AddUserProfilePreference("printing.print_preview_sticky_settings.appState", "{\"recentDestinations\":[{\"id\":\"Save as PDF\",\"origin\":\"local\",\"account\":\"\"}],\"selectedDestinationId\":\"Save as PDF\",\"version\":2}");
            return chromeOptions;
        }

        private static ChromeDriverService GetChromeDriverService()
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            return chromeDriverService;
        }

        private static PrintOptions.PageSize GetPageSize(PaperKind size)
        {
            return size switch
            {
                PaperKind.A4 => new() { WidthInInches = 8.3, HeightInInches = 11.7 },
                PaperKind.Letter => new() { WidthInInches = 8.5, HeightInInches = 11 },
                _ => new() { WidthInInches = 8.5, HeightInInches = 11 }
            };
        }

        private static PrintOptions.PageSize GetPageSize(string? size)
        {
            return size?.ToLower() switch
            {
                "a4" => new() { WidthInInches = 8.3, HeightInInches = 11.7 },
                "f4" => new() { WidthInInches = 8.3, HeightInInches = 13 },
                "letter" => new() { WidthInInches = 8.5, HeightInInches = 11 },
                _ => new() { WidthInInches = 8.5, HeightInInches = 11 }
            };
        }

        private static PrintOrientation GetPageOrientation(string? orientation)
        {
            return orientation?.ToLower() switch
            {
                "portrait" => PrintOrientation.Portrait,
                "landscape" => PrintOrientation.Landscape,
                _ => PrintOrientation.Portrait
            };
        }

        private static PrintOptions GetPrintOptions(PrintOptions.PageSize size, PrintOrientation orientation)
        {
            PrintOptions printOptions = new()
            {
                Orientation = orientation,
                OutputBackgroundImages = true,
            };
            printOptions.PageDimensions.Width = size.Width;
            printOptions.PageDimensions.Height = size.Height;

            return printOptions;
        }

        public static async Task ConvertHtmlToPdfAsync(string htmlFilePath, string outputPdfPath)
        {
            if (DriverState != DriverStates.Initialized || _driver == null) return;

            HtmlDocument doc = new();
            doc.Load(Path.GetFullPath(htmlFilePath));
            string content = doc.Text;

            string? size = doc.DocumentNode.SelectSingleNode($"//meta[@name='doc-size']")?.GetAttributeValue("content", null) ?? null;
            string? orientation = doc.DocumentNode.SelectSingleNode($"//meta[@name='doc-orientation']")?.GetAttributeValue("content", null) ?? null;


            await Task.Run(() =>
            {
                try
                {
                    _driver.Navigate().GoToUrl($"file:///{Path.GetFullPath(htmlFilePath)}");
                    PrintOptions printOptions = GetPrintOptions(GetPageSize(size), GetPageOrientation(orientation));
                    OpenQA.Selenium.PrintDocument doc = _driver.Print(printOptions);
                    doc.SaveAsFile(outputPdfPath);
                }
                catch (Exception ex)
                {
                    _crashed = true;
                    Trace.TraceError($"An error occurred during HTML to PDF conversion: {ex.Message}");
                }
            });
            
        }

        public static void Dispose()
        {
            lock (Lock)
            {
                if (_driver == null) return;

                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}
