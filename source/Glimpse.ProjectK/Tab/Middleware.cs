using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.ProjectK.Middleware;
using Microsoft.AspNet.Http;

namespace Glimpse.Owin.Tab
{
    public class Middleware : TabBase, IKey
    {
        public override string Name
        {
            get { return "Middleware"; }
        }

        public override object GetData(ITabContext context)
        {
            var httpContext = context.GetRequestContext<HttpContext>();

            var tracker = httpContext.Items["glimpse.MiddlewareTracker"] as MiddlewareTracker;

            return tracker.Graph;
        }

        public string Key 
        {
            get { return "glimpse_middleware"; }
        }
    }
}
