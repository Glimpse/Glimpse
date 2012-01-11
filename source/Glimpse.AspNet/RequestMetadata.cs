using System;
using System.Diagnostics.Contracts;
using System.Web;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class RequestMetadata : IRequestMetadata
    {
        private HttpContextBase Context { get; set; }

        public RequestMetadata(HttpContextBase context)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");

            Context = context;
        }

        public string RequestUri
        {
            get { return Context.Request.Url.AbsoluteUri; }
        }
        public string RequestHttpMethod
        {
            get { return Context.Request.HttpMethod; }
        }
        public string GetCookie(string name)
        {
            var cookie = Context.Request.Cookies.Get(name);

            return cookie == null ? null : cookie.Value;
        }

        public int ResponseStatusCode
        {
            get { return Context.Response.StatusCode; }
        }
        public string ResponseContentType
        {
            get { return Context.Response.ContentType; }
        }
        public string IpAddress { get
        {
            throw new NotImplementedException("Need to implement this IP logic");
        }
        }
        public bool RequestIsAjax
        {
            get
            {
                var request = Context.Request;

                if (request["X-Requested-With"] == "XMLHttpRequest")
                    return true;
                if (request.Headers != null)
                    return request.Headers["X-Requested-With"] == "XMLHttpRequest";

                return false;
            }
        }
    }
}