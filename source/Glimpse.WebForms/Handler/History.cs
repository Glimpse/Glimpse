using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.WebForms.Extensibility;
using Glimpse.WebForms.Plumbing;
using Newtonsoft.Json;

namespace Glimpse.WebForms.Handler
{
    [GlimpseHandler]
    public class History : JsonHandlerBase
    {
        [ImportingConstructor]
        public History(JsonSerializerSettings jsonSerializerSettings) : base(jsonSerializerSettings){}

        public override string ResourceName
        {
            get { return "History"; }
        }

        protected override object GetData(HttpContext context)
        {
            var queue = context.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
            if (queue != null)
            {
                var result = new Dictionary<string, object>();
                IEnumerable<GlimpseRequestMetadata> data;

                var requestId = context.Request.QueryString[GlimpseConstants.ClientRequestId];
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
                    var clientName = context.Request.QueryString[GlimpseConstants.ClientName];

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

                return new {Data = result};
            }
            
            return new {Error = true, Message = "No history avalible."};
        }
    }
}