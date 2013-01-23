namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Allows a <see cref="ITab"/> to define what URI it should use if the Tab has supporting
    /// documentation.
    /// </summary>
    /// <remarks>
    /// If this interface isn't implemented on a <see cref="ITab"/> no documentation will be 
    /// provided.
    /// </remarks>
    public interface IDocumentation
    {
        /// <summary>
        /// Gets the documentation URI.
        /// </summary>
        /// <value>The documentation URI.</value>
        string DocumentationUri { get; }
    }
}
