using System;
using Glimpse.Core.Framework;
using Microsoft.Owin;

namespace Glimpse.Owin.Middleware
{
    public class RequestMetadata : IRequestMetadata
    {
        private readonly OwinRequest request;
        private readonly OwinResponse response;

        public RequestMetadata(OwinRequest request, OwinResponse response)
        {
            this.request = request;
            this.response = response;
        }

        public string RequestUri 
        {
            get { return request.Uri.AbsoluteUri; }
        }

        public string RequestHttpMethod 
        {
            get { return request.Method; }
        }

        public int ResponseStatusCode 
        {
            get { return response.StatusCode; }
        }

        public string ResponseContentType 
        {
            get { return response.ContentType; }
        }

        public bool RequestIsAjax 
        {
            get
            {
                if (request.Headers != null)
                {
                    return request.Headers["X-Requested-With"] == "XMLHttpRequest";
                }

                return false;
            }
        }

        public string ClientId 
        {
            get
            {
                if (request.User != null && !string.IsNullOrEmpty(request.User.Identity.Name))
                {
                    return request.User.Identity.Name;
                }

                return Guid.NewGuid().ToString("N");
            }
        }

        public string GetCookie(string name)
        {
            return request.Cookies[name];
        }

        public string GetHttpHeader(string name)
        {
            return request.Headers[name];
        }
    }
}