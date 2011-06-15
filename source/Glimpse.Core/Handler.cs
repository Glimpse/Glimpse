using System;
using System.Linq;
using System.Web;

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
            //TODO: Validation/security check!
            var queryString = context.Request.QueryString;

            var resource = string.IsNullOrWhiteSpace(queryString[ResourceKey]) ? "Config" : queryString[ResourceKey];

            var handler = Module.Handlers.Where(h=>h.ResourceName.Equals(resource, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (handler == null)
            {
                context.Response.StatusCode = 404;
                context.Response.End();
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
