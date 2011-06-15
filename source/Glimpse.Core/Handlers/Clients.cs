using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class Clients:JsonHandlerBase{

        public IGlimpseMetadataStore MetadataStore { get; set; }

        [ImportingConstructor]
        public Clients(GlimpseSerializer serializer, IGlimpseMetadataStore metadataStore) : base(serializer)
        {
            MetadataStore = metadataStore;
        }

        public override string ResourceName
        {
            get { return "Clients"; }
        }

        protected override object GetData(HttpContextBase context)
        {
            if (MetadataStore.Requests.Count() != 0)
            {
                var result = new Dictionary<string, object>();
                var sortedRequests = from request in MetadataStore.Requests orderby request.ClientName select request;
                var lastClient = Guid.NewGuid().ToString();

                foreach (var request in sortedRequests)
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