namespace Glimpse.WebForms.Responder
{ 
    [GlimpseResponder]
    public class Sprite : ImageResponder
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