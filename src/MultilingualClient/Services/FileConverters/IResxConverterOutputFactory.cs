namespace ResxConverter.Core
{
    /// <summary>
    /// Represents a factory for the converter output.
    /// </summary>
    public interface IResxConverterOutputFactory
    {
        /// <summary>
        /// Create the <see cref="IResxConverterOutput"/> for the specified culture and outputFolder.
        /// </summary>
        /// <param name="culture">Culture.</param>
        /// <param name="outputFolder">Output folder.</param>
        IResxConverterOutput Create(string culture, string outputFolder);
    }
}
