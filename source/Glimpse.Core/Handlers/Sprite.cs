using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{ 
    [GlimpseHandler]
    public class Sprite : ImageHandlerBase
    {
        protected override string EmbeddedResourceName
        {
            get { return "Glimpse.Core.glimpseSprite.png"; }
        }

        public override string ResourceName
        {
            get { return "sprite.png"; }
        }

        protected override string ContentType
        {
            get { return "image/png"; }
        }
    } 
}