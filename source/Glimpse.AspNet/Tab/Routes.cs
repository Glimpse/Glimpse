using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Routes : AspNetTab, IDocumentation
    {
        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/Routes"; }
        }

        public override string Name
        {
            get { return "Routes"; }
        }

        public override object GetData(ITabContext context)
        {
            var result = new List<RouteInstance>();

            using (RouteTable.Routes.GetReadLock())
            {
                var requestContext = context.GetRequestContext<HttpContextBase>();

                if (requestContext == null)
                {
                    return null;
                }

                result.AddRange(RouteTable.Routes.Select(routeBase => new RouteInstance(routeBase, requestContext)));
            }

            return result;
        }
    }
}