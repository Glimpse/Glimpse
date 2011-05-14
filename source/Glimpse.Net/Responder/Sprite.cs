namespace Glimpse.Net.Responder
{ 
    [GlimpseResponder]
    public class Sprite : BaseImageResonder
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