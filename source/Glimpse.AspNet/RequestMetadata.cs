using System;
using System.Web;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class RequestMetadata : IRequestMetadata
    {
        public RequestMetadata(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Context = context;
        }

        public string RequestHttpMethod
        {
            get { return Context.Request.HttpMethod; }
        }

        public int ResponseStatusCode
        {
            get { return Context.Response.StatusCode; }
        }

        public string ResponseContentType
        {
            get { return Context.Response.ContentType; }
        }

        public bool RequestIsAjax
        {
            get
            {
                var request = Context.Request;

                if (request["X-Requested-With"] == "XMLHttpRequest")
                {
                    return true;
                }

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
                if (Context.User != null && Context.User.Identity != null)
                {
                    var user = Context.User.Identity.Name;
                    if (!string.IsNullOrEmpty(user))
                    {
                        return user;
                    }
                }

                var browser = Context.Request.Browser;

                if (browser != null)
                {
                    return string.Format("{0} {1}", browser.Browser, browser.Version);
                }

                return Guid.NewGuid().ToString("N");
            }
        }

        public string RequestUri
        {
            get { return Context.Request.Url.AbsoluteUri; }
        }

        private HttpContextBase Context { get; set; }

        public string GetCookie(string name)
        {
            var cookie = Context.Request.Cookies.Get(name);

            return cookie == null ? null : cookie.Value;
        }

        public string GetHttpHeader(string name)
        {
            return Context.Request.Headers.Get(name);
        }
    }
}