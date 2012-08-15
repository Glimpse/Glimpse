using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Glimpse.AspNet.Extensibility;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.Tab
{
    public class Routes : AspNetTab, IDocumentation
    {
        public override object GetData(ITabContext context)
        {
            var result = new List<RouteModel>();

            var hasEverMatched = false;
            using (RouteTable.Routes.GetReadLock())
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                foreach (RouteBase routeBase in RouteTable.Routes)
                {
                    var routeData = routeBase.GetRouteData(httpContext);
                    bool matchesCurrentRequest = (routeData != null);

                    var route = routeBase as Route;

                    if (route != null)
                    {
                        RouteValueDictionary values = null;
                        if (routeData != null) values = routeData.Values;

                        var data = new List<RouteDataItemModel>();

                        if (values != null && route.Defaults != null)
                        {
                            data.AddRange(
                                from defaultItem in route.Defaults
                                select new RouteDataItemModel(defaultItem.Key, 
                                    values[defaultItem.Key], defaultItem.Value));
                        }

                        var area = "_Root_";

                        if (route.DataTokens != null && route.DataTokens.ContainsKey("area"))
                            area = route.DataTokens["area"].ToString();

                        result.Add(new RouteModel(matchesCurrentRequest, area,
                                            route.Url, data.Count > 1 ? data : null,
                                            (route.Constraints == null || route.Constraints.Count == 0)
                                                ? null
                                                : route.Constraints,
                                            (route.DataTokens == null || route.DataTokens.Count == 0)
                                                ? null
                                                : route.DataTokens,
                                            matchesCurrentRequest && !hasEverMatched
                                        ));
                    }
                    else
                    {
                        result.Add(new RouteModel(matchesCurrentRequest));
                    }

                    hasEverMatched = hasEverMatched || matchesCurrentRequest;
                }
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