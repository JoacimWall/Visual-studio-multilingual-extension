using System.Xml.Linq;
using MultilingualExtension.Shared.Interfaces;

namespace MultilingualExtension.Shared.Services.FileConverters
{
    /// <summary>
    /// Converts RESX files using outputs created by the suplied <see cref="IResxConverterOutputFactory"/>.
    /// </summary>
    public sealed class ResxConverter
    {
        private readonly IResxConverterOutputFactory _outputFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ResxConverter.Core.ResxConverter"/> class.
        /// </summary>
        /// <param name="outputFactory">Output factory.</param>
        public ResxConverter(IResxConverterOutputFactory outputFactory)
        {
            if (outputFactory == null)
            {
                throw new ArgumentNullException(nameof(outputFactory));
            }

            _outputFactory = outputFactory;
        }

        /// <summary>
        /// Convert the resx files specified in the inputFolder and place the converter files into outputFolder.
        /// </summary>
        /// <param name="inputFolder">Input folder.</param>
        /// <param name="outputFolder">Output folder.</param>
        public void Convert(string inputFolder, string outputFolder,IStatusPadLoger outputPane)
        {
            if (inputFolder == null)
            {
                throw new ArgumentNullException(nameof(inputFolder));
            }

            if (outputFolder == null)
            {
                throw new ArgumentNullException(nameof(outputFolder));
            }

            var resxPerCulture = Directory.EnumerateFiles(inputFolder, "*.resx", SearchOption.AllDirectories)
                  .Select(path => new
                  {
                      Culture = Path.GetFileNameWithoutExtension(path).GetExtensionWitoutDot(),
                      FileName = Path.GetFileName(path),
                      FilePath = path
                  })
                  .GroupBy(resxCulture => resxCulture.Culture);

            foreach (var resxGroup in resxPerCulture)
            {
                using (var output = _outputFactory.Create(resxGroup.Key, outputFolder))
                {
                    foreach (var resxCulture in resxGroup)
                    {
                        // Writes the file we are converting into a comment.
                        output.WriteComment(resxCulture.FileName);

                        foreach (var node in XDocument.Load(resxCulture.FilePath).DescendantNodes())
                        {
                            var element = node as XElement;
                            var comment = node as XComment;

                            if (element?.Name == "data")
                            {
                                output.WriteString(new ResxString
                                {
                                    Key = element.Attribute("name").Value,
                                    Value = element.Value.Trim()
                                });
                            }
                            else if (comment != null)
                            {
                                output.WriteComment(comment.Value);
                            }
                        }
                        outputPane.WriteText(string.Format("Create native file for {0}", resxCulture.FileName));
                    }
                }
            }
        }
    }
}

