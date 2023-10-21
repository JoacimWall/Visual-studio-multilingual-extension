using System;

namespace ResxConverter.Core
{
    /// <summary>
    /// RESX converter output factory.
    /// </summary>
    public sealed class ResxConverterOutputFactory : IResxConverterOutputFactory
    {
        private readonly Func<string, string, IResxConverterOutput> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ResxConverter.Core.ResxConverterOutputFactory"/> class.
        /// </summary>
        /// <param name="factory">Factory.</param>
        public ResxConverterOutputFactory(Func<string, string, IResxConverterOutput> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _factory = factory;
        }

        /// <summary>
        /// Create the <see cref="IResxConverterOutput"/> from the specified culture and outputFolder.
        /// </summary>
        /// <param name="culture">Culture.</param>
        /// <param name="outputFolder">Output folder.</param>
        public IResxConverterOutput Create(string culture, string outputFolder) => _factory(culture, outputFolder);
    }
}
