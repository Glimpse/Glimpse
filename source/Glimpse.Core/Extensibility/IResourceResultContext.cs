using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IResourceResultContext</c> provides implementations of <see cref="IResourceResult"/> access to the
    /// <see cref="IFrameworkProvider"/>, <see cref="ISerializer"/> and <see cref="IHtmlEncoder"/>.
    /// </summary>
    public interface IResourceResultContext : IContext
    {
        /// <summary>
        /// Gets the framework provider.
        /// </summary>
        /// <value>The framework provider.</value>
        IRequestResponseAdapter RequestResponseAdapter { get; }

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