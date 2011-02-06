using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class RouteData:IGlimpsePlugin
    {
        public string Name
        {
            get { return "RouteData"; }
        }

        public object GetData(HttpApplication application)
        {
            var routeData = new Dictionary<string, string>();
            var counter = 1;
            foreach (var item in RouteTable.Routes)
            {
                routeData.Add(counter++.ToString(), item.ToString());
            }

            if (routeData.Count == 0) return null;

            return routeData;
        }
    }
}
