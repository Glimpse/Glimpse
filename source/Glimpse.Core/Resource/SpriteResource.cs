using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    public class SpriteResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_sprite";

        public SpriteResource()
        {
            ResourceName = "Glimpse.Core.sprite.png";
            ResourceType = "image/png";
            Name = InternalName;
        }

        public string Key
        {
            get { return Name; }
        }
    }
}