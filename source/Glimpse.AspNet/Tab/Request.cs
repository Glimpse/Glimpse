using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Extensions;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Request:AspNetTab, IDocumentation
    {
        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetHttpContext();

            var cookies = GetCookies(httpContext);


            return new RequestModel
                       {
                           Cookies = cookies
                       };

        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string DocumentationUri
        {
            //TODO: Update to proper Uri
            get { return "http://localhost/someUrl"; }
        }

        private ICollection<Cookie> GetCookies(HttpContextBase context)
        {
            var cookies = context.Request.Cookies;

            var result = new List<Cookie>();

            foreach (var key in cookies.AllKeys)
            {
                var cookie = cookies[key];

                result.Add(new Cookie { Name = cookie.Name, Path = cookie.Path, IsSecure = cookie.Secure, Value = context.Server.UrlDecode(cookie.Value) });
            }

            return result;
        }
    }

    public class RequestModel
    {
        public ICollection<Cookie> Cookies { get; set; }


    }

    public class Cookie
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsSecure { get; set; }
        public string Value { get; set; }
    }
}
