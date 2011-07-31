using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class History : JsonHandlerBase
    {
        [Import]
        internal GlimpseConfiguration Configuration { get; set; }
        public const string ClientName = "ClientName";
        public const string ClientRequestId = "ClientRequestID";
        public IGlimpseMetadataStore MetadataStore { get; set; }

        [ImportingConstructor]
        public History(GlimpseSerializer serializer, IGlimpseMetadataStore metadataStore) : base(serializer)
        {
            MetadataStore = metadataStore;
        }

        public override string ResourceName
        {
            get { return "History"; }
        }

        protected override object GetData(HttpContextBase context)
        {
            var response = context.Response;
            if (MetadataStore.Requests.Count() != 0)
            {
                var result = new Dictionary<string, object>();
                IEnumerable<GlimpseRequestMetadata> data;

                var requestId = context.Request.QueryString[ClientRequestId];
                if (!string.IsNullOrEmpty(requestId))
                { 
                    data = from request in MetadataStore.Requests
                           where request.RequestId.ToString().Equals(requestId)
                           select request;

                    var requestResult = data.FirstOrDefault(); 
                    if (requestResult != null) 
                        result.Add(requestResult.RequestId.ToString(), new { Data = requestResult.Json });

                    //We don't want to cache named requests as its results change over time, this one doesn't
                    if (Configuration.CacheEnabled)
                        response.ExpiresAbsolute = DateTime.Now.AddMonths(6);
                }
                else
                {
                    var clientName = context.Request.QueryString[ClientName];

                    //Removed as we don't want everything to be returned in the case where the user hasn't typed a name yet @nikmd23 to confirm
                    //if (string.IsNullOrEmpty(clientName))
                    //    data = MetadataStore.Requests;
                    //else
                    //{
                        data = from request in MetadataStore.Requests
                               where request.ClientName.Equals(clientName)
                               select request;
                    //}

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
            
            return new {Error = true, Message = "No history available."};
        }
    }
}