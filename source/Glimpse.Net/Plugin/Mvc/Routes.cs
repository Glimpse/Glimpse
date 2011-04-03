using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Net.Plumbing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Routes:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Routes"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var requestContext = store[GlimpseConstants.RequestContext] as RequestContext;
            if (requestContext == null) return null;

            var result = new List<object[]>
                             {
                                 new[] {"Match", "Url", "Defaults", "Constraints", "DataTokens"}
                             };

            var routeData = requestContext.RouteData;
            //TODO: Show these values somehow
            var routeValues = routeData.Values;
            var matchedRouteBase = routeData.Route;

            using (RouteTable.Routes.GetReadLock())
            {
                foreach (RouteBase routeBase in RouteTable.Routes)
                {
                    bool matchesCurrentRequest = (routeBase.GetRouteData(requestContext.HttpContext) != null);

                    var route = routeBase as Route;
                    if (route != null)
                    {
                        result.Add(new object[]
                                       {
                                           matchesCurrentRequest.ToString(), 
                                           route.Url, route.Defaults,
                                           (route.Constraints == null ||route.Constraints.Count == 0) ? null : route.Constraints,
                                           (route.DataTokens == null || route.DataTokens.Count == 0) ? null : route.DataTokens,
                                           matchesCurrentRequest ? "selected" : "quiet"
                                       });
                    }
                    else
                    {
                        result.Add(new object[] {matchesCurrentRequest.ToString(), null, null, null, null});
                    }
                }
            }
           
            return result;
        }

        public void SetupInit(HttpApplication application)
        {
            var filters = GlobalFilters.Filters;

            if (!filters.OfType<GlimpseFilterAttribute>().Any())
                filters.Add(new GlimpseFilterAttribute(), int.MinValue);
        }
    }
}
