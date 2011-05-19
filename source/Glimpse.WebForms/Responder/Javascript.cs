using System.IO;
using System.Reflection;
using System.Web;
using Glimpse.WebForms.Configuration;

namespace Glimpse.WebForms.Responder
{
    [GlimpseResponder]
    public class Javascript : GlimpseResponder
    {
        public override string ResourceName
        {
            get { return "glimpseClient.js"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            var response = application.Response;
            var assembly = Assembly.GetExecutingAssembly();

            using (var resourceStream = assembly.GetManifestResourceStream("Glimpse.WebForms.glimpseClient.js"))
            {
                if (resourceStream != null)
                {
                    using (var reader = new StreamReader(resourceStream))
                    {
                        response.Write(reader.ReadToEnd());
                    }
                }
            }
            response.AddHeader("Content-Type", "application/x-javascript");
            application.CompleteRequest();
        }
    }
}