using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Sprite:GlimpseResponder
    {
        [ImportingConstructor]
        public Sprite(JavaScriptSerializer jsSerializer):base(jsSerializer){}

        public override string ResourceName
        {
            get { return "glimpseSprite.png"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            var response = application.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.Net.glimpseSprite.png"))
            {
                if (resourceStream != null)
                {
                    using (var bitmap = new Bitmap(resourceStream))
                    {
                        bitmap.Save(response.OutputStream, ImageFormat.Png);
                    }
                }
            }
            response.AddHeader("Content-Type", "image/png");
            application.CompleteRequest();

            return;
        }
    }
}