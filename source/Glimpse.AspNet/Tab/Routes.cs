using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    //TODO: Use AspTab
    public class Routes : TabBase<HttpContextBase>, IDocumentation
    {
        public override object GetData(ITabContext context)
        {
            var result = new List<RouteInstance>();

            using (RouteTable.Routes.GetReadLock())
            {
                var requestContext = context.GetRequestContext<HttpContextBase>();

                if (requestContext == null) 
                    return null;

                result.AddRange(RouteTable.Routes.Select(routeBase => new RouteInstance(routeBase, requestContext)));
            }

            return result;
        }

        public override string Name
        {
            get { return "Routes"; }
        }

        public string DocumentationUri
        {
            get { return "http://getGlimpse.com/Help/Plugin/Routes"; }
        }
    }
}