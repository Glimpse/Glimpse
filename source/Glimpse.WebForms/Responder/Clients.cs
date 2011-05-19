using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.WebForms.Configuration;
using Glimpse.WebForms.Extensions;
using Glimpse.WebForms.Plumbing;

namespace Glimpse.WebForms.Responder
{
    [GlimpseResponder]
    public class Clients:JsonResponder{

        public override string ResourceName
        {
            get { return "Clients"; }
        }

        protected override object GetData(HttpApplication application, GlimpseConfiguration config)
        {
            if (!application.IsValidRequest(config, false, checkPath: false))
            {
                return new {Error = true, Message = "You are not configured to access history."};
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

                    dictionary.Add(request.RequestId.ToString(), new {request.Url, request.Browser, request.RequestTime, request.IsAjax, request.Method});

                    lastClient = request.ClientName;
                }

                return new {Data = result};
            }
            
            return new {Error = true, Message = "No history avalible."};
        }
    }
}