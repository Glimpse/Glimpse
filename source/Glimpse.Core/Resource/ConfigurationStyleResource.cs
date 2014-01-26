using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse Configuration Style
    /// </summary>
    public class ConfigurationStyleResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_config_style";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationStyleResource" /> class.
        /// </summary>
        public ConfigurationStyleResource()
        {
            Name = InternalName;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Returns the embedded resource that represents the Glimpse Configuration Style which will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded Glimpse Configuration Style</returns>
        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return new EmbeddedResourceInfo(this.GetType().Assembly, "Glimpse.Core.EmbeddedResources.glimpse_config.css", "text/css");
        }
    }
}