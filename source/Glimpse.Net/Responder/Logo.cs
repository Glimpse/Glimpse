namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Logo : BaseImageResonder
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