namespace Glimpse.WebForms.Responder
{
    [GlimpseResponder]
    public class Logo : ImageResponder
    {
        public override string ResourceName
        {
            get { return "glimpseLogo.png"; }
        }

        public override string ContentType
        {
            get { return "image/png"; }
        } 
    }
}