using System;

namespace Glimpse.Core.Framework
{
    public class GlimpseRequestHeaders
    {
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

        private GlimpseRequest GlimpseRequest { get; set; }
    }
}