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
        private static readonly List<string> IgnoredRouteTypes = new List<string> { "System.Web.Http.WebHost.Routing.HttpWebRoute" };
         
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
