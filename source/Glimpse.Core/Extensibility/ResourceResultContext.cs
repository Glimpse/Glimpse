using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The implementation of <see cref="IResourceResultContext"/> used in the <c>Execute</c> method of <see cref="IResourceResult"/>.
    /// </summary>
    public class ResourceResultContext : IResourceResultContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceResultContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="frameworkProvider">The framework provider.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="htmlEncoder">The HTML encoder.</param>
        public ResourceResultContext(ILogger logger, IFrameworkProvider frameworkProvider, ISerializer serializer, IHtmlEncoder htmlEncoder)
        {
            Logger = logger;
            FrameworkProvider = frameworkProvider;
            Serializer = serializer;
            HtmlEncoder = htmlEncoder;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the framework provider.
        /// </summary>
        /// <value>
        /// The framework provider.
        /// </value>
        public IFrameworkProvider FrameworkProvider { get; set; }

        /// <summary>
        /// Gets or sets the serializer.
        /// </summary>
        /// <value>
        /// The serializer.
        /// </value>
        public ISerializer Serializer { get; set; }

        /// <summary>
        /// Gets or sets the HTML encoder.
        /// </summary>
        /// <value>
        /// The HTML encoder.
        /// </value>
        public IHtmlEncoder HtmlEncoder { get; set; }
    }
}