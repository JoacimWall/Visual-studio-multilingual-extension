using System.IO;

namespace ResxConverter.Core
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Gets the file extension witout dot.
        /// </summary>
        /// <returns>The extension witout dot.</returns>
        /// <param name="fileName">File name.</param>
        public static string GetExtensionWitoutDot(this string fileName) => Path.GetExtension(fileName).Replace(".", string.Empty);
    }
}
