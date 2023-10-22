namespace MultilingualExtension.Shared.Services.FileConverters
{
    /// <summary>
    /// RESX converters for mobile platforms.
    /// </summary>
    /// <see cref="Core.ResxConverter"/>
    public static class ResxConverters
    {
        /// <summary>
        /// Converter for Android. Produces <code>strings.xml</code> files.
        /// </summary>
        public static readonly ResxConverter Android = new ResxConverter(new ResxConverterOutputFactory((culture, outputFolder) => new AndroidResxConverterOutput(outputFolder, culture)));

        /// <summary>
        /// Converter for iOS. Produces <code>Localizable.strings</code> files.
        /// </summary>
        public static readonly ResxConverter iOS = new ResxConverter(new ResxConverterOutputFactory((culture, outputFolder) => new iOSResxConverterOutput(outputFolder, culture)));
    }
}
