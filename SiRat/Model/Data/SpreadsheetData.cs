using OfficeOpenXml;
using SiRat.Data;
using SiRat.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiRat.Model.Data
{
    /// <summary>
    /// Represents data from a spreadsheet file.
    /// </summary>
    public class SpreadsheetData
    {
        private Dictionary<string, Dictionary<string, List<string?>>> _data { get; set; }
        /// <summary>
        /// Gets the full path of the spreadsheet file.
        /// </summary>
        public string FullPath { get; }

        /// <summary>
        /// Gets the name of the file including the extension.
        /// </summary>
        public string FileName => Path.GetFileName(FullPath);

        /// <summary>
        /// Gets the name of the file without the extension.
        /// </summary>
        public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FullPath);

        /// <summary>
        /// Gets the parent directory of the file.
        /// </summary>
        public string? ParentDirectory => Directory.GetParent(FullPath)?.FullName;

        /// <summary>
        /// Gets the parent <see cref="ReportData"/> of the spreadsheet.
        /// </summary>
        public ReportData? Parent => GetParent();

        /// <summary>
        /// Gets the raw data stored in the spreadsheet.
        /// </summary>
        public Dictionary<string, Dictionary<string, List<string?>>> Raw => _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetData"/> class with specified path and data.
        /// </summary>
        /// <param name="path">The full path of the spreadsheet file.</param>
        /// <param name="data">The data to initialize with.</param>
        public SpreadsheetData(string path, Dictionary<string, Dictionary<string, List<string?>>> data)
        {
            FullPath = path;
            _data = data;
        }

        /// <summary>
        /// Gets the parent <see cref="ReportData"/> for this spreadsheet data.
        /// </summary>
        /// <returns>The parent report data.</returns>
        public ReportData? GetParent()
        {
            return GlobalData.SantriList.SelectMany(santri => santri.Reports).FirstOrDefault(report => report.SpreadsheetData.Equals(this));
        }

        /// <summary>
        /// Creates a new instance of <see cref="SpreadsheetData"/> from the specified spreadsheet file.
        /// </summary>
        /// <param name="pathname">The full path of the spreadsheet file.</param>
        /// <returns>A new instance of <see cref="SpreadsheetData"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the file is not a valid spreadsheet.</exception>
        public static SpreadsheetData FromSpreadsheet(string pathname)
        {
            if (Path.GetExtension(pathname) != ".xlsx") throw new ArgumentException("File is not a spreadsheet");
            Dictionary<string, Dictionary<string, List<string?>>> data = new();
            FileInfo fileInfo = new(pathname);
            if (ExcelPackage.LicenseContext == null) ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using ExcelPackage package = new(fileInfo);
            foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
            {
                string sheetName = worksheet.Name;
                Dictionary<string, List<string?>> sheet = new();

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    string? colName = worksheet.Cells[1, col].Value?.ToString();
                    if (colName == null) continue;

                    List<string?> values = new();
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) values.Add(worksheet.Cells[row, col].Value?.ToString());
                    sheet.Add(colName, values);
                }

                data.Add(sheetName, sheet);
            }

            return new SpreadsheetData(pathname, data);
        }

        /// <summary>
        /// Executes a query or a function on the data.
        /// </summary>
        /// <param name="query">The query string to execute.</param>
        /// <returns>The result of the query or function execution.</returns>
        public object? Query(string query)
        {
            Match functionMatch = Regex.Match(query, @"(\w+)\((.*)\)");
            if (!functionMatch.Success) return ExecuteQuery(query);

            string functionName = functionMatch.Groups[1].Value;
            string functionArgs = functionMatch.Groups[2].Value;

            string[] arguments = functionArgs.Split(',').ToArray();

            return ExecuteFunction(functionName, arguments);
        }

        private object? ExecuteFunction(string functionName, string[] arguments)
        {
            return functionName.ToUpper() switch
            {
                "SUMACROSS" => SumAcross(arguments[0]),
                "AVGACROSS" => AvgAcross(arguments[0]),
                "SPELL" => Spell(arguments[0]),
                "EQ" => Eq(arguments),
                "GRADE" => Grade(arguments[0]),
                "REVERSE" => Reverse(arguments[0]),
                "ARABIC" => Arabic(arguments[0]),
                _ => null
            };
        }

        private object? ExecuteQuery(string query)
        {
            string[] arguments = query.Split('.').Take(2).ToArray();
            Match match;

            match = Regex.Match(arguments[0], "\"(.*?)\"");
            if (!match.Success || !_data.ContainsKey(match.Groups[1].Value)) return null;

            Dictionary<string, List<string?>> targetTable = _data[match.Groups[1].Value];
            if (arguments.Length == 1) return targetTable;

            match = Regex.Match(arguments[1], "\"(.*?)\"");
            if (!match.Success || !targetTable.ContainsKey(match.Groups[1].Value)) return targetTable;

            List<string?> targetColumn = targetTable[match.Groups[1].Value];

            match = Regex.Match(arguments[1], @"\[(\d+)\]");
            if (!match.Success || !int.TryParse(match.Groups[1].Value, out int valueIndex) || targetColumn.Count <= valueIndex) return targetColumn;

            return targetColumn[valueIndex];
        }

        private double? SumAcross(string query)
        {
            if (Parent == null) return null;

            SpreadsheetData? template = Parent?.TemplateData;
            List<SpreadsheetData> data = GlobalData.SantriList.SelectMany(santri => santri.Reports).Where(report => report.TemplateData?.Equals(template) == true).Select(report => report.SpreadsheetData).ToList();
            List<string?> queried = data.Select(sp => sp.Query(query)).Where(res => res is string).Select(q => q as string).ToList();
            double sum = queried.Where(value => double.TryParse(value, out _)).Select(value => double.Parse(value!)).Sum();

            return sum;
        }

        private double? AvgAcross(string query)
        {
            if (Parent == null) return null;

            SpreadsheetData? template = Parent?.TemplateData;
            List<SpreadsheetData> data = GlobalData.SantriList.SelectMany(santri => santri.Reports).Where(report => report.TemplateData?.Equals(template) == true).Select(report => report.SpreadsheetData).ToList();
            List<string?> queried = data.Select(sp => sp.Query(query)).Where(res => res is string).Select(q => q as string).ToList();

            try
            {
                double avg = queried.Where(value => double.TryParse(value, out _)).Select(value => double.Parse(value!)).Average();

                return avg;
            }
            catch
            {
                return 0;
            }
            
        }

        private string? Spell(string query)
        {
            string? num = (string?)Query(query);
            if (num == null) return null;

            string spelling = new Regex(@"(^[a-z])|\s+(.)", RegexOptions.ExplicitCapture).Replace(new IndonesianSpelling(int.Parse(num)).ToString().ToLower(), s => s.Value.ToUpper());
            return spelling;
        }

        private string? Eq(string[] args)
        {
            if (args.Length < 3) return null;
            string? query = (string?)Query(args[0]);
            if (query == null) return null;
            if (query.Equals(args[1])) return args[2];
            return (args.Length >= 4) ? args[3] : "";
        }

        private string? Grade(string query)
        {
            string? q = (string?)Query(query);
            if (q == null) return null;

            double num = double.Parse(q);
            if (num >= 91) return "A";
            if (num >= 81) return "B";
            if (num >= 71) return "C";
            return "K";
        }

        private string? Reverse(string query)
        {
            string? str = (string?)Query(query);
            if (str == null) return null;

            return string.Join("", str.ToCharArray().Reverse());
        }

        private string? Arabic(string query)
        {
            string? input = (string?)Query(query);
            if (input == null) return null;

            string pattern = @"\d+";
            var matches = Regex.Matches(input, pattern);

            var parts = new List<string>();
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                if (match.Index > lastIndex) parts.Add(input.Substring(lastIndex, match.Index - lastIndex));
                parts.Add(match.Value);
                lastIndex = match.Index + match.Length;
            }

            if (lastIndex < input.Length) parts.Add(input.Substring(lastIndex));

            parts.Reverse();
            return string.Join("", parts);
        }
    }
}
