using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.WebForms.Configuration;
using Glimpse.WebForms.Extensions;
using Glimpse.WebForms.Plumbing;
using Newtonsoft.Json;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class History : GlimpseResponder
    {
        public override string ResourceName
        {
            get { return "History"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            if (!application.IsValidRequest(config, false, checkPath: false))
            {
                var data =
                    JsonConvert.SerializeObject(new {Error = true, Message = "You are not configured to access history."}, Formatting.None);
                JsonResponse(application, data);
                return;
            }

            var queue = application.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
            if (queue != null)
            {
                var result = new Dictionary<string, object>();
                IEnumerable<GlimpseRequestMetadata> data;

                var requestId = application.Request.QueryString[GlimpseConstants.ClientRequestId];
                if (!string.IsNullOrEmpty(requestId))
                { 
                    data = from request in queue
                           where request.RequestId.ToString().Equals(requestId)
                           select request;

                    var requestResult = data.FirstOrDefault(); 
                    if (requestResult != null) 
                        result.Add(requestResult.RequestId.ToString(), new { Data = requestResult.Json });  
                }
                else
                {
                    var clientName = application.Request.QueryString[GlimpseConstants.ClientName];

                    if (string.IsNullOrEmpty(clientName))
                        data = queue;
                    else
                    {
                        data = from request in queue
                               where request.ClientName.Equals(clientName)
                               select request;
                    }

                    var lastClient = Guid.NewGuid().ToString();
                    foreach (var request in data.OrderBy(d => d.ClientName))
                    {
                        if (!lastClient.Equals(request.ClientName))
                            result.Add(request.ClientName, new Dictionary<string, object>());

                        var dictionary = result[request.ClientName] as IDictionary<string, object>;
                        dictionary.Add(request.RequestId.ToString(), new { Data = request.Json });

                        lastClient = request.ClientName;
                    }

                    if (!string.IsNullOrEmpty(clientName) && result.Count == 0)
                        result.Add(clientName, new Dictionary<string, object>()); 
                }

                var json = JsonConvert.SerializeObject(new { Data = result }, Formatting.None);
                JsonResponse(application, json);
                return; 
            }
            else
            {
                var data = JsonConvert.SerializeObject(new {Error = true, Message = "No history avalible."}, Formatting.None);
                JsonResponse(application, data);
                return;
            }
        }
    }
}