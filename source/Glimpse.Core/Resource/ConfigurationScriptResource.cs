using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse Configuration Script
    /// </summary>
    public class ConfigurationScriptResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_config_script";

        private EmbeddedResourceInfo Info { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationScriptResource" /> class.
        /// </summary>
        public ConfigurationScriptResource()
        {
            Name = InternalName;
            Info = new EmbeddedResourceInfo(this.GetType().Assembly, "EmbeddedResources/glimpse_config.js", "text/javascript");
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
        /// Returns the embedded resource that represents the Glimpse Configuration Script which will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded Glimpse Configuration Script</returns>
        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return Info;
        }
    }
}