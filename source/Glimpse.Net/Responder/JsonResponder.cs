using System.Web;
using Glimpse.WebForms.Configuration;
using Newtonsoft.Json;

namespace Glimpse.WebForms.Responder
{
    public abstract class JsonResponder:GlimpseResponder
    {
        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            var data = GetData(application, config);
            var dataString = JsonConvert.SerializeObject(data, Formatting.None);

            var response = application.Response;
            response.Write(dataString);
            response.AddHeader("Content-Type", "application/json");
            application.CompleteRequest();
        }

        protected abstract object GetData(HttpApplication application, GlimpseConfiguration config);
    }
}