using System;
using System.Linq;
using System.Web;
using System.Web.Util;
using Glimpse.Core.Validator;

namespace Glimpse.Core
{
    public class Handler:IHttpHandler
    {
        internal const string ResourceKey = "r";

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        internal static void ProcessRequest(HttpContextBase context)
        {
            if (!Module.RequestValidator.IsValid(context, LifecycleEvent.Handler))
            {
                context.Response.StatusCode = 403;
                context.Response.Write("<html><head><title>403 Forbidden</title></head><body><h1>403 Forbidden</h1>Ensure '<em>"+ context.Request.UserHostAddress+"</em>' is configured for Glimpse access.</body></html>");
                return;
            }

            //TODO: Validation/security check!
            var queryString = context.Request.QueryString;

            var resource = string.IsNullOrWhiteSpace(queryString[ResourceKey]) ? "Config" : queryString[ResourceKey];

            var handler = Module.Handlers.Where(h=>h.ResourceName.Equals(resource, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (handler == null)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("<html><head><title>404 Not Found</title></head><body><h1>404 Not Found</h1>Could not find Glimpse resource with name '<em>" + resource + "</em>'.</body></html>");
                return;
            }

            handler.ProcessRequest(context);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
