using System.ComponentModel.Composition;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{ 
    [GlimpseResponder]
    public class Sprite : BaseImageResonder
    {
        [ImportingConstructor]
        public Sprite(JavaScriptSerializer jsSerializer) : base(jsSerializer) { }

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