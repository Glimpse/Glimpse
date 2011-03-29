using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    public abstract class GlimpseResponder
    {
        public static JavaScriptSerializer JsSerializer { get; set; }

        protected GlimpseResponder(JavaScriptSerializer jsSerializer)
        {
            JsSerializer = jsSerializer;
        }

        protected static void JsonResponse(HttpApplication httpApplication, string data)
        {
            var response = httpApplication.Response;
            response.Write(data);
            response.AddHeader("Content-Type", "application/json");
            httpApplication.CompleteRequest();
        }

        public abstract string ResourceName { get; }
        public abstract void Respond(HttpApplication application, GlimpseConfiguration config);
    }
}