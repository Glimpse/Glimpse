using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Metadata
{
    public class CorrelationInstanceMetadata : IInstanceMetadata
    {
        private const string UriId = "GlimpsePrgLocation";
        private const string RequestId = "GlimpsePrgId";
        private const string MethodId = "GlimpsePrgMethod";

        public string Key
        {
            get { return "correlation"; }
        }

        public object GetInstanceMetadata(IConfiguration configuration, IGlimpseRequestContext requestContext)
        {
            var requestResponseAdapter = requestContext.RequestResponseAdapter;
            var requestMetdata = requestResponseAdapter.RequestMetadata;

            if (requestMetdata.ResponseStatusCode == 301 || requestMetdata.ResponseStatusCode == 302)
            {
                requestResponseAdapter.SetCookie(RequestId, requestContext.GlimpseRequestId.ToString());
                requestResponseAdapter.SetCookie(MethodId, requestMetdata.RequestHttpMethod);
                requestResponseAdapter.SetCookie(UriId, requestMetdata.RequestUri.PathAndQuery);
            }
            else if (!string.IsNullOrEmpty(requestMetdata.GetCookie(RequestId)))
            {
                var correlation = new
                {
                    Title = string.Format("{0}R{1} Request", requestMetdata.GetCookie(MethodId).TakeFirstChar(), requestMetdata.RequestHttpMethod.TakeFirstChar()),
                    Legs = new List<object> 
                    {
                        new { RequestId = requestMetdata.GetCookie(RequestId), Method = requestMetdata.GetCookie(MethodId), Uri = requestMetdata.GetCookie(UriId) },
                        new { RequestId = requestContext.GlimpseRequestId.ToString(), Method = requestMetdata.RequestHttpMethod, Uri = requestMetdata.RequestUri.PathAndQuery }
                    }
                };

                requestResponseAdapter.SetCookie(UriId, null);
                requestResponseAdapter.SetCookie(RequestId, null);
                requestResponseAdapter.SetCookie(MethodId, null);

                return correlation;
            }

            return null;
        }
    }
}
