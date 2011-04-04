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
    public class History : GlimpseResponder
    {
        [ImportingConstructor]
        public History(JavaScriptSerializer jsSerializer) : base(jsSerializer)
        {
        }

        public override string ResourceName
        {
            get { return "History"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            if (!application.IsValidRequest(config, false, checkPath: false))
            {
                var data =
                    JsSerializer.Serialize(new {Error = true, Message = "You are not configured to access history."});
                JsonResponse(application, data);
                return;
            }

            var queue = application.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
            if (queue != null)
            {
                var clientName = application.Request.QueryString[GlimpseConstants.ClientName];
                var result = new Dictionary<string, object>();
                IEnumerable<GlimpseRequestMetadata> data;

                if (string.IsNullOrEmpty(clientName))
                    data = queue;
                else
                {
                    data = from request in queue
                                        where request.ClientName.Equals(clientName)
                                        select request;
                }

                var lastClient = Guid.NewGuid().ToString();
                foreach (var request in data.OrderBy(d=> d.ClientName))
                {
                    if (!lastClient.Equals(request.ClientName))
                        result.Add(request.ClientName, new Dictionary<string, object>());

                    var dictionary = result[request.ClientName] as IDictionary<string, object>;
                    dictionary.Add(request.RequestId.ToString(), new {Data = request.Json});

                    lastClient = request.ClientName;
                }

                if (!string.IsNullOrEmpty(clientName) && result.Count == 0)
                    result.Add(clientName, new Dictionary<string, object>());

                var json = JsSerializer.Serialize(new {Data = result});
                JsonResponse(application, json);
                return;
            }
            else
            {
                var data = JsSerializer.Serialize(new {Error = true, Message = "No history avalible."});
                JsonResponse(application, data);
                return;
            }
        }
    }
}