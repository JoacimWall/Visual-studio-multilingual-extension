using System;
using System.Text.RegularExpressions;

namespace MultilingualExtension.Helper
{
    public static class RexExHelper
    {
        public static Match ValidateFilenameIsTargetType(string path)
        {
            Regex regex = new Regex(".[a-zA-Z][a-zA-Z]-[a-zA-Z][a-zA-Z].resx$");
            return regex.Match(path);

        }
    }
}
