namespace MultilingualExtension.Shared.Services.FileConverters
{
    /// <summary>
    /// Represents how a string and comment must be materialized. <see cref="Commit"/> after all content is written; disposing without it discards the output.
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

        /// <summary>
        /// Writes the accumulated content to the output file.
        /// </summary>
        void Commit();
    }
}
