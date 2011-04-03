using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;
using Glimpse.Net.Plumbing;

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
                var result = new Dictionary<string, object>();
                var sortedQueue = from request in queue orderby request.ClientName select request;
                var lastClient = Guid.NewGuid().ToString();

                foreach (var request in sortedQueue)
                {
                    if (!lastClient.Equals(request.ClientName))
                        result.Add(request.ClientName, new Dictionary<string, object>());

                    var dictionary = result[request.ClientName] as IDictionary<string, object>;
                    dictionary.Add(request.RequestId.ToString(), new {request.Browser, request.RequestTime, request.IsAjax});

                    lastClient = request.ClientName;
                }

                var data = JsSerializer.Serialize(new {Data = result});
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