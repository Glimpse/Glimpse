using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Inspector
{
    public class RoutesInspector : IInspector
    {
        private static readonly FieldInfo MappedRoutesField = typeof(System.Web.Routing.RouteCollection).GetField("_namedMap", BindingFlags.NonPublic | BindingFlags.Instance);
         
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;
            var alternateBaseImplementationMvc = new AlternateType.RouteBase(context.ProxyFactory, context.Logger);
            var alternateBaseImplementationWebApi = new AlternateType.HttpRoute(context.ProxyFactory, context.Logger); 

            var currentMvcRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentMvcRoutes.GetWriteLock())
            {
                var mappedRoutes = (Dictionary<string, System.Web.Routing.RouteBase>)MappedRoutesField.GetValue(currentMvcRoutes);

                for (var i = 0; i < currentMvcRoutes.Count; i++)
                {
                    var originalObj = currentMvcRoutes[i];

                    if (originalObj.GetType() != typeof(System.Web.Routing.Route)) continue;

                    var newObj = (System.Web.Routing.RouteBase)null;

                    var mixins = new[] { RouteNameMixin.None() };
                    var routeName = string.Empty; 
                    if (mappedRoutes.ContainsValue(originalObj))
                    {
                        var pair = mappedRoutes.First(r => r.Value == originalObj);
                        routeName = pair.Key;
                        mixins = new[] { new RouteNameMixin(pair.Key) };
                    }

                    if (alternateBaseImplementationMvc.TryCreate(originalObj, out newObj, mixins))
                    {
                        currentMvcRoutes[i] = newObj;

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


            using (var currentWebApiRoutes = GlobalConfiguration.Configuration.Routes)
            {
                var mappedRoutes = (Dictionary<string, System.Web.Http.Routing.IHttpRoute>)MappedRoutesField.GetValue(currentWebApiRoutes);

                for (var i = 0; i < currentWebApiRoutes.Count; i++)
                {
                    var originalObj = currentWebApiRoutes[i];

                    if (originalObj.GetType() != typeof(System.Web.Http.Routing.HttpRoute)) continue;

                    var newObj = (System.Web.Http.Routing.IHttpRoute)null;

                    var mixins = new[] { RouteNameMixin.None() };
                    var routeName = string.Empty;
                    if (mappedRoutes.Any(r => r.Value == originalObj))
                    {
                        var pair = mappedRoutes.First(r => r.Value == originalObj);
                        routeName = pair.Key;
                        mixins = new[] { new RouteNameMixin(pair.Key) };
                    }

                    var routeType = originalObj.GetType();

                    if (routeType == typeof(System.Web.Http.Routing.HttpRoute) && alternateBaseImplementationWebApi.TryCreate(originalObj, out newObj, mixins))
                    {
                        currentWebApiRoutes.Remove(routeName);
                        currentWebApiRoutes.Add(routeName, newObj);

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
