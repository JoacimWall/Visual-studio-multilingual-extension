namespace ResxConverter.Mobile
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
        public static readonly Core.ResxConverter Android = new Core.ResxConverter(new Core.ResxConverterOutputFactory((culture, outputFolder) => new AndroidResxConverterOutput(outputFolder, culture)));

        /// <summary>
        /// Converter for iOS. Produces <code>Localizable.strings</code> files.
        /// </summary>
        public static readonly Core.ResxConverter iOS = new Core.ResxConverter(new Core.ResxConverterOutputFactory((culture, outputFolder) => new iOSResxConverterOutput(outputFolder, culture)));
    }
}
