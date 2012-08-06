using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Framework
{
    public class GlimpseRequestHeaders
    {
        private GlimpseRequest GlimpseRequest { get; set; }

        public GlimpseRequestHeaders(GlimpseRequest glimpseRequest)
        {
            GlimpseRequest = glimpseRequest;
        }

        public string ClientId
        {
            get { return GlimpseRequest.ClientId; }
        }
        public DateTime DateTime
        {
            get { return GlimpseRequest.DateTime; }
        }
        public long Duration
        {
            get { return GlimpseRequest.Duration; }
        }
        public Guid? ParentRequestId
        {
            get { return GlimpseRequest.ParentRequestId; }
        }
        public Guid RequestId
        {
            get { return GlimpseRequest.RequestId; }
        }
        public bool RequestIsAjax
        {
            get { return GlimpseRequest.RequestIsAjax; }
        }
        public string RequestHttpMethod
        {
            get { return GlimpseRequest.RequestHttpMethod; }
        }
        public string RequestUri
        {
            get { return GlimpseRequest.RequestUri; }
        }
        public string ResponseContentType
        {
            get { return GlimpseRequest.ResponseContentType; }
        }
        public int ResponseStatusCode
        {
            get { return GlimpseRequest.ResponseStatusCode; }
        }
        public string UserAgent
        {
            get { return GlimpseRequest.UserAgent; }
        }
    }

    public class GlimpseRequest
    {
        public GlimpseRequest(Guid requestId, IRequestMetadata requestMetadata, IDictionary<string, TabResult> pluginData, long duration)
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
            ClientId = requestMetadata.GetCookie(Constants.ClientIdCookieName) ?? requestMetadata.ClientId;
            UserAgent = requestMetadata.GetHttpHeader(Constants.UserAgentHeaderName);

            Guid parentRequestId;

#if NET35
            if (RequestIsAjax && Glimpse.Core2.Backport.Net35Backport.TryParseGuid(requestMetadata.GetHttpHeader(Constants.HttpRequestHeader), out parentRequestId))
                ParentRequestId = parentRequestId;
#else
            if (RequestIsAjax && Guid.TryParse(requestMetadata.GetHttpHeader(Constants.HttpRequestHeader), out parentRequestId))
                ParentRequestId = parentRequestId;
#endif
        }

        public string ClientId { get; set; }
        public DateTime DateTime { get; set; }
        public long Duration { get; set; }
        public Guid? ParentRequestId { get; set; }
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