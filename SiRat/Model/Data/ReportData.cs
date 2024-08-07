using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Model.Data
{
    /// <summary>
    /// Represents a report consisting of spreadsheet data, format data, and template data.
    /// </summary>
    public class ReportData
    {
        /// <summary>
        /// Gets or sets the parent <see cref="Santri"/> for the report.
        /// </summary>
        public Santri? Parent;

        /// <summary>
        /// Gets or sets the <see cref="SpreadsheetData"/> for the report.
        /// </summary>
        public SpreadsheetData SpreadsheetData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FormatData"/> for the report.
        /// </summary>
        public FormatData? FormatData { get; set; }

        /// <summary>
        /// Gets or sets the template <see cref="SpreadsheetData"/> used in the report.
        /// </summary>
        public SpreadsheetData? TemplateData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportData"/> class with specified spreadsheet data.
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet data.</param>
        public ReportData(SpreadsheetData spreadsheet)
        {
            SpreadsheetData = spreadsheet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportData"/> class with specified spreadsheet and format data.
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet data.</param>
        /// <param name="format">The format data.</param>
        public ReportData(SpreadsheetData spreadsheet, FormatData format)
        {
            SpreadsheetData = spreadsheet;
            FormatData = format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportData"/> class with specified spreadsheet, format, and template data.
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet data.</param>
        /// <param name="format">The format data.</param>
        /// <param name="template">The template data.</param>
        public ReportData(SpreadsheetData spreadsheet, FormatData format, SpreadsheetData template)
        {
            SpreadsheetData = spreadsheet;
            FormatData = format;
            TemplateData = template;
        }
    }
}
