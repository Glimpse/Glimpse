using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Clients:GlimpseResponder{

        [ImportingConstructor]
        public Clients(JavaScriptSerializer jsSerializer):base(jsSerializer){}

        public override string ResourceName
        {
            get { return "Clients"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            if (!application.IsValidRequest(config, false, checkPath: false))
            {
                var data =
                    JsSerializer.Serialize(new { Error = true, Message = "You are not configured to access history." });
                JsonResponse(application, data);
                return;
            }

            var queue = application.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
            if (queue != null)
            {
                var filteredQueue = from request in queue
                                    group request by request.ClientName
                                        into clients
                                        select new { Client = clients.Key, RequestCount = clients.Count() };

                var response = new List<object[]>{new[]{"Client", "Count"}};
                response.AddRange(filteredQueue.Select(client => new object[] {client.Client, client.RequestCount}));

                var data = JsSerializer.Serialize(response);
                JsonResponse(application, data);
                return;
            }
            else
            {
                var data = JsSerializer.Serialize(new { Error = true, Message = "No history avalible." });
                JsonResponse(application, data);
                return;
            }
        }
    }
}