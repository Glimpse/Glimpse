using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata(Guid requestId, IRequestMetadata requestMetadata, IDictionary<string, TabResult> pluginData, long duration)
        {
            RequestId = requestId;
            PluginData = pluginData;
            Duration = duration;

            DateTime = DateTime.Now;
            RequestHttpMethod = requestMetadata.RequestHttpMethod;
            RequestIsAjax = requestMetadata.RequestIsAjax;
            RequestUri = requestMetadata.RequestUri;
            ResponseStatusCode = requestMetadata.ResponseStatusCode;
            ResponseContentType = requestMetadata.ResponseContentType;
            ClientId = requestMetadata.GetCookie(Constants.ClientIdCookieName);
            UserAgent = requestMetadata.GetHttpHeader(Constants.UserAgentHeaderName);

            Guid parentRequestId;
            if (RequestIsAjax && Guid.TryParse(requestMetadata.GetHttpHeader(Constants.HttpRequestHeader), out parentRequestId))
                ParentRequestId = parentRequestId;
        }

        public string ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public long Duration { get; set; }
        public Guid ParentRequestId { get; set; }
        public Guid RequestId { get; set; }
        public bool RequestIsAjax { get; set; }
        public string RequestHttpMethod { get; set; }
        public string RequestUri { get; set; }
        public string ResponseContentType { get; set; }
        public int ResponseStatusCode { get; set; }
        public IDictionary<string, TabResult> PluginData { get; set; }
        public string UserAgent { get; set; }
    }
}