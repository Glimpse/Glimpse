using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.WebApi.Core.Model;
using Glimpse.Core.Extensibility;

namespace Glimpse.WebApi.WebHost.Inspector
{
    public class RoutesInspector : IInspector
    {
        private static readonly FieldInfo MappedRoutesField = typeof(System.Web.Routing.RouteCollection).GetField("_namedMap", BindingFlags.NonPublic | BindingFlags.Instance);
        private List<System.Web.Routing.RouteBase> oldRoutes = new List<System.Web.Routing.RouteBase>();
        private Dictionary<string, System.Web.Http.Routing.IHttpRoute> alternateHttpRoutes = new Dictionary<string, System.Web.Http.Routing.IHttpRoute>();
        
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;
            var alternateBaseImplementation = new IHttpRoute(context.ProxyFactory, context.Logger);

            using (var currentHttpRoutes = GlobalConfiguration.Configuration.Routes)
            {
                var currentRoutes = System.Web.Routing.RouteTable.Routes;
                
                using (currentRoutes.GetWriteLock())
                {
                    var mappedRoutes = (Dictionary<string, System.Web.Routing.RouteBase>)MappedRoutesField.GetValue(currentRoutes);
    
                    for (var i = 0; i < currentRoutes.Count; i++)
                    {
                        var originalObj = currentRoutes[i];
                        if (originalObj.GetType().ToString() != "System.Web.Http.WebHost.Routing.HttpWebRoute")
                        {
                            oldRoutes.Add(originalObj);
                            continue;
                        }
    
                        var mixins = new[] { RouteNameMixin.None() };
                        var routeName = string.Empty; 
                        if (mappedRoutes.ContainsValue(originalObj))
                        {
                            var pair = mappedRoutes.First(r => r.Value == originalObj);
                            routeName = pair.Key;
                            mixins = new[] { new RouteNameMixin(pair.Key) };
                        }

                        System.Web.Http.Routing.IHttpRoute originalObjHttpRoute;
                        currentHttpRoutes.TryGetValue(routeName, out originalObjHttpRoute);

                        if (originalObjHttpRoute.GetType().ToString() == "System.Web.Http.Routing.RouteCollectionRoute")
                        {
                            // This catches any routing that has been defined using Attribute Based Routing
                            // System.Web.Http.Routing.RouteCollectionRoute is a collection of HttpRoutes

                            var subRoutes = originalObjHttpRoute.GetType().GetField("_subRoutes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(originalObjHttpRoute);
                            var routes = (IList<System.Web.Http.Routing.IHttpRoute>)subRoutes.GetType().GetField("_routes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(subRoutes);

                            for (var j = 0; j < routes.Count; j++)
                            {
                                var route = routes[j];
                                var newObj = (System.Web.Http.Routing.IHttpRoute)null;

                                if (alternateBaseImplementation.TryCreate(route, out newObj, mixins))
                                {
                                    routes[j] = newObj;
                                    logger.Info(Resources.RouteSetupReplacedRoute, originalObj.GetType());
                                }
                                else
                                {
                                    logger.Info(Resources.RouteSetupNotReplacedRoute, originalObj.GetType());
                                }
                                
                            }

                            alternateHttpRoutes.Add(routeName, originalObjHttpRoute);
                        }
                        else
                        {
                            CreateAlternateType(originalObj, originalObjHttpRoute, logger, alternateBaseImplementation, mixins, routeName);
                        }
                    }
                }
                
                // The WebAPI HttpRouteCollection is converted to a HostedHttpRouteCollection,
                // which doesn't have a Remove() method (it throws an exception). So instead create
                // a local copy of all the HttpRoutes, and then use the Clear() and Add() methods
                // to update the HostedHttpRouteCollection with the proxied routes

                currentHttpRoutes.Clear();
                
                foreach(var altHttpRoute in alternateHttpRoutes)
                {
                    currentHttpRoutes.Add(altHttpRoute.Key, altHttpRoute.Value);
                }

                // Calling the Clear() method on the HttpRouteCollection flows down and also 
                // clears the RouteCollection, which might also contain MVC routes etc, so we need
                // to re-add them too

                foreach (var route in oldRoutes)
                {
                    currentRoutes.Add(route);
                }

            }
        }

        public void CreateAlternateType(System.Web.Routing.RouteBase originalObj, System.Web.Http.Routing.IHttpRoute originalObjHttpRoute, ILogger logger, Glimpse.WebApi.Core.AlternateType.IHttpRoute alternateBaseImplementation, RouteNameMixin[] mixins, string routeName)
        {
            var newObj = (System.Web.Http.Routing.IHttpRoute)null;

            if (alternateBaseImplementation.TryCreate(originalObjHttpRoute, out newObj, mixins))
            {
                if (!string.IsNullOrEmpty(routeName))
                {
                    alternateHttpRoutes.Add(routeName, newObj);
                    //mappedRoutes[routeName] = newObj;
                }

                logger.Info(Resources.RouteSetupReplacedRoute, originalObj.GetType());
            }
            else
            {
                if (!string.IsNullOrEmpty(routeName))
                {
                    alternateHttpRoutes.Add(routeName, originalObjHttpRoute);
                }

                logger.Info(Resources.RouteSetupNotReplacedRoute, originalObj.GetType());
            }

        }
    }
}
