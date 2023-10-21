using System;

namespace ResxConverter.Core
{
    /// <summary>
    /// Represents how a string and comment must be materialized. Disposed after all content is written.
    /// </summary>
    public interface IResxConverterOutput : IDisposable
    {
        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="stringElement">String. <see cref="ResxString"/></param>
        void WriteString(ResxString stringElement);

        /// <summary>
        /// Writes a comment.
        /// </summary>
        /// <param name="comment">Comment.</param>
        void WriteComment(string comment);
    }
}
