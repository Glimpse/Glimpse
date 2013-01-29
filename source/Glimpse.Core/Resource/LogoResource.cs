using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the Glimpse logo.
    /// </summary>
    public class LogoResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_logo";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoResource" /> class.
        /// </summary>
        public LogoResource()
        {
            ResourceName = "Glimpse.Core.logo.png";
            ResourceType = "image/png";
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