using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse JavaScript client to the browser.
    /// </summary>
    public class InsightResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_insight";

        private EmbeddedResourceInfo ResourceInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientResource" /> class.
        /// </summary>
        public InsightResource()
        {
            Name = InternalName;

            ResourceInfo = new EmbeddedResourceInfo(GetType().Assembly, "Glimpse.Core.glimpseInsight.js", "application/x-javascript");
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
        /// Returns the embedded resource that represents the Glimpse Client which will be returned during the execution of the <see cref="FileResource"/>
        /// </summary>
        /// <param name="context">The resource context</param>
        /// <returns>Information about the embedded Glimpse Client</returns>
        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return ResourceInfo;
        }
    }
}