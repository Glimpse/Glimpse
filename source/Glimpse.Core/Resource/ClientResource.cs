using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse JavaScript client to the browser.
    /// </summary>
    public class ClientResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_client";

        private EmbeddedResourceInfo Info { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientResource" /> class.
        /// </summary>
        public ClientResource()
        {
            Name = InternalName;
            Info = new EmbeddedResourceInfo(this.GetType().Assembly, "glimpse.js", "application/x-javascript");
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
            return Info;
        }
    }
}