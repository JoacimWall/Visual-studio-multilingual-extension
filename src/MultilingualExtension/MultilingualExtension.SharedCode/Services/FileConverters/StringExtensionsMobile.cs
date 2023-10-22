using System.Text.RegularExpressions;

namespace MultilingualExtension.Shared.Services.FileConverters
{
    /// <summary>
    /// Extensions for <see cref="string"/>.
    /// </summary>
    public static class StringExtensionsMobile
    {
        private const string CamelCaseSeparatorRegexString = @"
            # Not start
            (?<!^)
            # If previous is upper-case letter: match upper-case letter followed by lower-case letter (e.g. 'G' in HTMLGuide)
            # Else (previous is lower-case): match upper-case letter
            (?(?<=\p{Lu})\p{Lu}(?=\p{Ll})|\p{Lu})
        ";

        private static readonly Regex CamelCaseSeparatorRegex = new Regex(CamelCaseSeparatorRegexString, RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Convert camel-case to lower-case separated with underscore.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Lower case underscore string.</returns>
        public static string ToLowerUnderScoreFromCamelCase(this string value) => CamelCaseSeparatorRegex.Replace(value, "_$0").ToLower();

        /// <summary>
        /// Escapes the " ' \ \n characteres.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="includeSingleQuotes">True to escape single quotes; false otherwise.</param>
        /// <returns>Escpaed string</returns>
        public static string EscapeSpecialCharacters(this string value, bool includeSingleQuotes)
        {
            // Search for " ' \ or \n using the regex "|\\|\n|'
            var regex = "\"|\\\\|\n";
            if (includeSingleQuotes)
            {
                regex += "|'";
            }

            return Regex.Replace(value, regex, EscapeSpecialCharacters);
        }

        private static string EscapeSpecialCharacters(Match m) => m.Value == "\n" ? "\\n" : '\\' + m.Value;
    }
}
