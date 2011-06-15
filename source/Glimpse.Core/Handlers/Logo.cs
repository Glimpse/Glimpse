using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class Logo : ImageHandlerBase
    {
        protected override string EmbeddedResourceName
        {
            get { return "Glimpse.Core.glimpseLogo.png"; }
        }

        public override string ResourceName
        {
            get { return "logo.png"; }
        }

        protected override string ContentType
        {
            get { return "image/png"; }
        } 
    }
}