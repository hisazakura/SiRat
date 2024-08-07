using HtmlAgilityPack;
using SiRat.Model.Data;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiRat.Services.Data
{
    public class ReportDataUtils
    {
        public static HtmlDocument ApplyFormat(FormatData format, SpreadsheetData data)
        {
            string content = format.Format.Text;
            MatchCollection matches = Regex.Matches(content, "{(.*?)}");

            foreach (Match match in matches)
            {
                string placeholder = match.Value;
                string query = match.Groups[1].Value;
                object? result = data.Query(query);

                if (result is double) result = result.ToString();
                if (result is not string) result = "";
                content = content.Replace(placeholder, result as string);
            }

            HtmlDocument doc = new();
            doc.LoadHtml(content);
            return doc;
        }
    }
}
