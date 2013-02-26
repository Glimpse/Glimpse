using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse JavaScript client to the browser.
    /// </summary>
    public class ClientResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_client";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientResource" /> class.
        /// </summary>
        public ClientResource()
        {
            ResourceName = "Glimpse.Core.glimpse.js";
            ResourceType = @"application/x-javascript";
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
    }
}