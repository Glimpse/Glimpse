using System.ComponentModel.Composition;
using System.Web.Script.Serialization;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Logo : BaseImageResonder
    {
        [ImportingConstructor]
        public Logo(JavaScriptSerializer jsSerializer) : base(jsSerializer) { }

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