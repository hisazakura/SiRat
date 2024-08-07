using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiRat.Model.Data
{
    /// <summary>
    /// Represents data from a format (HTML) file.
    /// </summary>
    public class FormatData
    {
        /// <summary>
        /// Gets the full path of the format file.
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
        /// Gets or sets the HTML document representing the format data.
        /// </summary>
        public HtmlDocument Format = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatData"/> class with specified path and document.
        /// </summary>
        /// <param name="pathname">The full path of the format file.</param>
        /// <param name="doc">The HTML document.</param>
        public FormatData(string pathname, HtmlDocument doc)
        {
            FullPath = pathname;
            Format = doc;
        }

        /// <summary>
        /// Creates a new instance of <see cref="FormatData"/> from the specified format file.
        /// </summary>
        /// <param name="pathname">The full path of the format file.</param>
        /// <returns>A new instance of <see cref="FormatData"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the file is not a valid HTML file.</exception>
        public static FormatData FromFormat(string pathname)
        {
            if (Path.GetExtension(pathname) != ".html") throw new ArgumentException("File is not an HTML file");
            HtmlDocument doc = new();
            doc.Load(pathname);

            return new FormatData(pathname, doc);
        }

        /// <summary>
        /// Retrieves the content of a meta tag by its name.
        /// </summary>
        /// <param name="name">The name attribute of the meta tag.</param>
        /// <returns>The content attribute of the meta tag, or null if not found.</returns>
        public string? GetMeta(string name)
        {
            HtmlNode metaTag = Format.DocumentNode.SelectSingleNode($"//meta[@name='{name}']");
            if (metaTag == null) return null;
            return metaTag.GetAttributeValue("content", null);
        }
    }
}
