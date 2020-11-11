using System;
using System.Text.RegularExpressions;

namespace MultilingualExtension.Shared.Helpers
{
    public static class RexExHelper
    {
        public static Match ValidateFileTypeIsResx(string path)
        {
            Regex regex = new Regex(@".resx$");
            return regex.Match(path);

        }
        public static Match ValidateFilenameIsTargetType(string path)
        {
            Regex regex = new Regex(@".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }
        public static Match ValidateFilenameIsTargetTypeCsv(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.csv$");
            return regex.Match(path);

        }
        public static Match ValidateFilenameIsTargetTypeXlsx(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx.xlsx$");
            return regex.Match(path);

        }
        public static Match GetFilenameResx(string path)
        {
            Regex regex = new Regex(@"\w+.[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }
    }
}
