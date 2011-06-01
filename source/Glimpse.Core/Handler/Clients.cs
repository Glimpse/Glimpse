using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;
using Newtonsoft.Json;

namespace Glimpse.Core.Handler
{
    [GlimpseHandler]
    public class Clients:JsonHandlerBase{

        [ImportingConstructor]//TODO:import full seralizer, not just settings
        public Clients(JsonSerializerSettings jsonSerializerSettings) : base(jsonSerializerSettings){}

        public override string ResourceName
        {
            get { return "Clients"; }
        }

        protected override object GetData(HttpContextBase context)
        {
            //TODO:Create IGlimpseMetadata store, and user via ImportingConstructor
            var queue = context.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
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
            
            return new {Error = true, Message = "No history available."};
        }
    }
}