namespace Glimpse.Core.Resource
{
    public class SpriteResource : FileResource
    {
        internal const string InternalName = "glimpse_sprite";

        public SpriteResource()
        {
            ResourceName = "Glimpse.Core.sprite.png";
            ResourceType = "image/png";
            Name = InternalName;
        }   
    }
}