using ResxConverter.Core;
using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace ResxConverter.Mobile
{
    /// <summary>
    /// <see cref="IResxConverterOutput"/> for Android. Produces Android <code>strings.xml</code> files.
    /// </summary>
    public class AndroidResxConverterOutput : IResxConverterOutput
    {
        /// <summary>
        /// The path of the output file represented by the current instance.
        /// </summary>
        public string OutputFilePath { get; }

        private readonly XDocument _xDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidResxConverterOutput"/> class.
        /// </summary>
        /// <param name="outputFolder">The base output folder. A culture-specific folder is created below this folder.</param>
        /// <param name="culture">The culture for this output.</param>
        public AndroidResxConverterOutput(string outputFolder, string culture)
        {
            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            OutputFilePath = GetOutputFilePath(outputFolder, culture);
            _xDocument = new XDocument(new XElement("resources"));
        }

        private static string GetOutputFilePath(string outputFolder, string culture)
        {
            var cultureSuffix = string.Empty;

            if (!string.IsNullOrEmpty(culture))
            {
                var cultureInfo = new CultureInfo(culture);
                if (cultureInfo.IsNeutralCulture)
                {
                    cultureSuffix = $"-{cultureInfo.TwoLetterISOLanguageName}";
                }
                else
                {
                    var regionInfo = new RegionInfo(cultureInfo.LCID);
                    cultureSuffix = $"-{cultureInfo.TwoLetterISOLanguageName}-r{regionInfo.TwoLetterISORegionName}";
                }
            }

            return Path.Combine(outputFolder, $"values{cultureSuffix}", "strings.xml");
        }

        /// <summary>
        /// Writes the accumulated data to the output file.
        /// </summary>
        public void Dispose()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
            _xDocument.Save(OutputFilePath);
        }

        /// <summary>
        /// Writes a comment.
        /// </summary>
        /// <param name="comment">Comment.</param>
        public void WriteComment(string comment)
        {
            _xDocument.Root.Add(new XComment(comment));
        }

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="stringElement">String. <see cref="ResxString"/></param>
        public void WriteString(ResxString stringElement)
        {
            _xDocument.Root.Add(CreateString(stringElement));
        }

        private XElement CreateString(ResxString stringElement)
        {
            var xStringElement = new XElement("string")
            {
                Value = stringElement.Value.EscapeSpecialCharacters(true)
            };
            xStringElement.SetAttributeValue("name", stringElement.Key.ToLowerUnderScoreFromCamelCase());
            return xStringElement;
        }
    }
}