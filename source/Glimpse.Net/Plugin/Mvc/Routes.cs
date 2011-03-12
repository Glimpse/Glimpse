using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
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
                                           (route.DataTokens == null || route.DataTokens.Count == 0) ? null : route.DataTokens
                                       });
                    }
                    else
                    {
                        result.Add(new object[] {matchesCurrentRequest.ToString(), null, null, null, null});
                    }
                }
            }

            /*string matchedRouteUrl = "n/a";

            string dataTokensRows = "";

            if (!(matchedRouteBase is DebugRoute))
            {
                foreach (string key in routeValues.Keys)
                {
                    routeDataRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeValues[key]);
                }

                foreach (string key in routeData.DataTokens.Keys)
                {
                    dataTokensRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeData.DataTokens[key]);
                }

                Route matchedRoute = matchedRouteBase as Route;

                if (matchedRoute != null)
                    matchedRouteUrl = matchedRoute.Url;
            }
            else
            {
                matchedRouteUrl = "<strong class=\"false\">NO MATCH!</strong>";
            }*/

            return result;
        }
    }
}
