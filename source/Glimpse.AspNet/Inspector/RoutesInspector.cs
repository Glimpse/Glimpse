using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Inspector
{
    public class RoutesInspector : IInspector
    {
        private static readonly FieldInfo MappedRoutesField = typeof(System.Web.Routing.RouteCollection).GetField("_namedMap", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly List<string> IgnoredRouteTypes = new List<string> { "System.Web.Http.WebHost.Routing.HttpWebRoute", "System.Web.Mvc.Routing.LinkGenerationRoute" };
         
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;
            var alternateBaseImplementation = new AlternateType.RouteBase(context.ProxyFactory, context.Logger); 

            var currentRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentRoutes.GetWriteLock())
            {
                var mappedRoutes = (Dictionary<string, System.Web.Routing.RouteBase>)MappedRoutesField.GetValue(currentRoutes);

                for (var i = 0; i < currentRoutes.Count; i++)
                {
                    var originalObj = currentRoutes[i];
                    if (IgnoredRouteTypes.Contains(originalObj.GetType().ToString()))
                    {
                        continue;
                    }

                    var newObj = (System.Web.Routing.RouteBase)null;
                    var mixins = new[] { RouteNameMixin.None() };
                    var routeName = string.Empty; 
                    if (mappedRoutes.ContainsValue(originalObj))
                    {
                        var pair = mappedRoutes.First(r => r.Value == originalObj);
                        routeName = pair.Key;
                        mixins = new[] { new RouteNameMixin(pair.Key) };
                    }


                    if (originalObj.GetType().ToString() == "System.Web.Mvc.Routing.RouteCollectionRoute")
                    {
                        // This catches any routing that has been defined using Attribute Based Routing
                        // System.Web.Mvc.Routing.RouteCollectionRoute is a collection of Routes

                        var subRoutes = originalObj.GetType().GetField("_subRoutes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(originalObj);
                        var routes = (IList<System.Web.Routing.Route>)subRoutes.GetType().GetField("_routes", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(subRoutes);

                        var alternateImplementation = new AlternateType.Route(context.ProxyFactory, context.Logger);

                        for (var j = 0; j < routes.Count; j++)
                        {
                            var route = routes[j];
                            var newSubRouteObj = (System.Web.Routing.Route)null;

                            if (alternateImplementation.TryCreate(route, out newSubRouteObj, mixins))
                            {
                                routes[j] = newSubRouteObj;
                                logger.Info(Resources.RouteSetupReplacedRoute, originalObj.GetType());
                            }
                            else
                            {
                                logger.Info(Resources.RouteSetupNotReplacedRoute, originalObj.GetType());
                            }

                        }
                    }

                    else
                    {
                        if (alternateBaseImplementation.TryCreate(originalObj, out newObj, mixins))
                        {
                            currentRoutes[i] = newObj;

                            if (!string.IsNullOrEmpty(routeName))
                            {
                                mappedRoutes[routeName] = newObj;
                            }

                            logger.Info(Resources.RouteSetupReplacedRoute, originalObj.GetType());
                        }
                        else
                        {
                            logger.Info(Resources.RouteSetupNotReplacedRoute, originalObj.GetType());
                        }
                    }
                }
            }
        }
    }
}
