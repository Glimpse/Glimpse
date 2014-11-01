using System;
using Glimpse.Core.Framework;
using Microsoft.AspNet.Http;

namespace Glimpse.AspNet5.Framework
{
    public class RequestMetadata : IRequestMetadata
    {
        private readonly HttpContext context;

        public RequestMetadata(HttpContext context)
        {
            this.context = context;
        }

        public Uri RequestUri
        {
            get { return new Uri(context.Request.Scheme + "://" + context.Request.Host + (context.Request.Path + context.Request.QueryString)); }
        }

        public string RequestHttpMethod
        {
            get { return context.Request.Method; }
        }

        public int ResponseStatusCode
        {
            get { return context.Response.StatusCode; }
        }

        public string ResponseContentType
        {
            get { return context.Response.ContentType; }
        }

        public bool RequestIsAjax
        {
            get
            {
                if (context.Request.Headers != null)
                {
                    return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                }

                return false;
            }
        }

        public string ClientId
        {
            get
            {
                if (context.User != null && !string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    return context.User.Identity.Name;
                }

                return Guid.NewGuid().ToString("N");
            }
        }

        public string GetCookie(string name)
        {
            return context.Request.Cookies[name];
        }

        public string GetHttpHeader(string name)
        {
            return context.Request.Headers[name];
        }
    }
}