namespace ResxConverter.Core
{
    /// <summary>
    /// Represents a RESX string.
    /// </summary>
    public class ResxString
    {
        /// <summary>
        /// The identifier of the RESX string entry.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value of the RESX string entry
        /// </summary>
        public string Value { get; set; }
    }
}
