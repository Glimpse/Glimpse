using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the context that is used when a resource result
    /// is executed.
    /// </summary>
    public interface IResourceResultContext : IContext
    {
        /// <summary>
        /// Gets the framework provider.
        /// </summary>
        /// <value>The framework provider.</value>
        IFrameworkProvider FrameworkProvider { get; }

        /// <summary>
        /// Gets the serializer.
        /// </summary>
        /// <value>The serializer.</value>
        ISerializer Serializer { get; }

        /// <summary>
        /// Gets the HTML encoder.
        /// </summary>
        /// <value>The HTML encoder.</value>
        IHtmlEncoder HtmlEncoder { get; }
    }
}