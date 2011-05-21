using Glimpse.WebForms.Extensibility;

namespace Glimpse.WebForms.Handler
{ 
    [GlimpseHandler]
    public class Sprite : ImageHandlerBase
    {
        public override string ResourceName
        {
            get { return "glimpseSprite.png"; }
        }

        public override string ContentType
        {
            get { return "image/png"; }
        }
    }
}