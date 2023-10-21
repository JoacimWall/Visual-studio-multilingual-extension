using ResxConverter.Core;
using System;
using System.IO;

namespace ResxConverter.Mobile
{
    /// <summary>
    /// iOS Resx converter output.
    /// </summary>
    public class iOSResxConverterOutput : IResxConverterOutput
    {
        /// <summary>
        /// The path of the output file represented by the current instance.
        /// </summary>
        public string OutputFilePath { get; }

        private readonly StreamWriter _streamWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="iOSResxConverterOutput"/> class.
        /// </summary>
        /// <param name="outputFolder">The base output folder. A culture-specific folder is created below this folder.</param>
        /// <param name="culture">The culture for this output.</param>
        public iOSResxConverterOutput(string outputFolder, string culture)
        {
            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            culture = string.IsNullOrEmpty(culture) ? "Base" : culture;
            OutputFilePath = Path.Combine(outputFolder, $"{culture}.lproj", "Localizable.strings");
            Directory.CreateDirectory(Path.GetDirectoryName(OutputFilePath));
            _streamWriter = new StreamWriter(OutputFilePath);
        }

        /// <summary>
        /// Flushes the accumulated data to the output file.
        /// </summary>
        public void Dispose() => _streamWriter.Dispose();

        /// <summary>
        /// Writes a comment.
        /// </summary>
        /// <param name="comment">Comment.</param>
        public void WriteComment(string comment) => _streamWriter.WriteLine($"/* {comment} */");

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="stringElement">String. <see cref="ResxString"/></param>
        public void WriteString(ResxString stringElement)
        {
            var key = stringElement.Key.ToLowerUnderScoreFromCamelCase();
            var value = stringElement.Value.EscapeSpecialCharacters(false);
            _streamWriter.WriteLine($"\"{key}\" = \"{value}\";");
        }
    }
}