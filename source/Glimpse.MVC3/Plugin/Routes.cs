using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.Mvc3.Plugin
{
    [GlimpsePlugin]
    internal class Routes:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Routes"; }
        }

        public object GetData(HttpApplication application)
        {
            var result = new List<object[]>
                             {
                                 new[] {"Match", "Area", "Url", "Data", "Constraints", "DataTokens"}
                             };

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

                        var data = new List<object[]>
                                       {
                                           new []{"Placeholder", "Default Value", "Actual Value"}
                                       };

                        if (values != null && route.Defaults != null)
                        {
                            foreach (var item in route.Defaults)
                            {
                                var @default = item.Value == UrlParameter.Optional ? "_Optional_" : item.Value;
                                var value = values[item.Key];
                                if (value != null) value = value == UrlParameter.Optional ? "_Optional_" : value;
                                data.Add(new []{item.Key, @default, value});
                            }
                        }

                        var area = "_Root_";

                        if(route.DataTokens != null && route.DataTokens.ContainsKey("area"))
                            area = route.DataTokens["area"].ToString();

                        result.Add(new object[]
                                       {
                                           matchesCurrentRequest.ToString(), area,
                                           route.Url, data.Count > 1 ? data : null,
                                           (route.Constraints == null ||route.Constraints.Count == 0) ? null : route.Constraints,
                                           (route.DataTokens == null || route.DataTokens.Count == 0) ? null : route.DataTokens,
                                           matchesCurrentRequest && !hasEverMatched ? "selected" : ""
                                       });
                    }
                    else
                    {
                        result.Add(new object[] {matchesCurrentRequest.ToString(), null, null, null, null});
                    }

                    hasEverMatched = hasEverMatched || matchesCurrentRequest;
                }
            }
           
            return result;
        }

        public void SetupInit(HttpApplication application)
        {
            throw new NotImplementedException();
        }
    }
}
