using System.Web;
using Glimpse.WebForms.Configuration;

namespace Glimpse.Net.Responder
{
    public abstract class GlimpseResponder
    {
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