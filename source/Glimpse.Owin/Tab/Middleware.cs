using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Owin.Middleware;

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
            var environment = context.GetRequestContext<IDictionary<string, object>>();

            var tracker = environment["glimpse.MiddlewareTracker"] as MiddlewareTracker;

            return tracker.Graph;
        }

        public string Key 
        {
            get { return "glimpse_middleware"; }
        }
    }
}
