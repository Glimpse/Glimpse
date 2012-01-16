using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Glimpse.AspNet.Model
{
    public class RouteInstance
    {
        public RouteInstance(RouteBase routeBase, HttpContextBase requestContext)
        {
            RouteType = routeBase.GetType();
            RouteData = routeBase.GetRouteData(requestContext);
            IsMatch = RouteData != null;
            Route = routeBase as Route;
        }

        //TODO: Underline?
        private const string DefaultAreaName = "Root";
        //TODO: technically Area's are an MVC concept. Can we get away with just showing data tokens?
        public string AreaName
        {
            get
            {
                var dataTokens = DataTokens;

                return (dataTokens != null && dataTokens.ContainsKey("area"))
                           ? dataTokens["area"].ToString()
                           : DefaultAreaName;
            }
        }

        //TODO: Check to see if constraints passed
        public RouteValueDictionary Constraints
        {
            get { return Route != null ? Route.Constraints : null; }
        }

        public RouteValueDictionary DataTokens
        {
            get { return Route != null ? Route.DataTokens : null; }
        }

        public bool IsMatch { get; private set; }

        internal Route Route { get; set; }

        internal RouteData RouteData { get; set; }

        public Type RouteType { get; private set; }

        private List<UriTokenInstance> tokens;

        public IList<UriTokenInstance> UriTokens
        {
            get
            {
                if (tokens != null) 
                    return tokens;

                var result = new List<UriTokenInstance>();

                RouteValueDictionary values = RouteData != null ? RouteData.Values : null;

                if (values != null && Route.Defaults != null)
                {
                    result.AddRange(Route.Defaults.Select(item => new UriTokenInstance
                                                                      {
                                                                          DefaultValue = item.Value,
                                                                          Value = values[item.Key],
                                                                          TokenName = item.Key
                                                                      }));
                }

                tokens = result;
                return result;
            }
        }

        public string UriTemplate
        {
            get { return Route != null ? Route.Url : null; }
        }
    }
}