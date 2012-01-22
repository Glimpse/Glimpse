using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseMetadata
    {
        public GlimpseMetadata(Guid requestId, IRequestMetadata requestMetadata, IDictionary<string, string> pluginData, long duration)
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

        string ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public long Duration { get; set; }
        public Guid ParentRequestId { get; set; }
        public Guid RequestId { get; set; }
        bool RequestIsAjax { get; set; }
        string RequestHttpMethod { get; set; }
        string RequestUri { get; set; }
        string ResponseContentType { get; set; }
        int ResponseStatusCode { get; set; }
        public IDictionary<string, string> PluginData { get; set; }
        public string UserAgent { get; set; }
    }
}