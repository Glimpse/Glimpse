namespace Glimpse.Core2.Resource
{
    public class SpriteResource:FileResource
    {
        internal const string InternalName = "glimpse-sprite";

        public SpriteResource()
        {
            ResourceName = "Glimpse.Core2.sprite.png";
            ResourceType = "image/png";
            Name = InternalName;
        }   
    }
}