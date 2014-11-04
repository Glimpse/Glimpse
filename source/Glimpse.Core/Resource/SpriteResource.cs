using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the image sprite containing user interface icons.
    /// </summary>
    public class SpriteResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_sprite";

        private EmbeddedResourceInfo Info { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteResource" /> class.
        /// </summary>
        public SpriteResource()
        {
            Name = InternalName;
            Info = new EmbeddedResourceInfo(this.GetType().Assembly, "EmbeddedResources/sprite.png", "image/png");
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

        protected override EmbeddedResourceInfo GetEmbeddedResourceInfo(IResourceContext context)
        {
            return Info;
        }
    }
}