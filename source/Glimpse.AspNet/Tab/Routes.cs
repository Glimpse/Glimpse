using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    [Tab(RequestContextType = typeof (HttpContextBase))]
    public class Routes : ITab, IDocumentation
    {
        public object GetData(ITabContext context)
        {
            var result = new List<RouteInstance>();

            using (RouteTable.Routes.GetReadLock())
            {
                var requestContext = context.GetRequestContext<HttpContextBase>();

                result.AddRange(RouteTable.Routes.Select(routeBase => new RouteInstance(routeBase, requestContext)));
            }

            return result;
        }

        public string Name
        {
            get { return "Routes"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/Routes"; }
        }
    }
}