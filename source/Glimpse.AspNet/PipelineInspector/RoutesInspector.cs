using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;
using Route = Glimpse.AspNet.AlternateType.Route;
using RouteBase = System.Web.Routing.RouteBase;

namespace Glimpse.AspNet.PipelineInspector
{
    public class RoutesInspector : IPipelineInspector
    {
        private static readonly FieldInfo MappedRoutesField = typeof(RouteCollection).GetField("_namedMap", BindingFlags.NonPublic | BindingFlags.Instance);

        public RoutesInspector()
        {
            RoutesConstraintInspector = new RoutesConstraintInspector();
        }

        private RoutesConstraintInspector RoutesConstraintInspector { get; set; }

        public void Setup(IPipelineInspectorContext context)
        { 
            var logger = context.Logger; 
            var alternateImplementation = new Route(context.ProxyFactory);
            var alternateBaseImplementation = new AlternateType.RouteBase(context.ProxyFactory);
            var alternateConstraintImplementation = new RouteConstraint(context.ProxyFactory);

            var currentRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentRoutes.GetWriteLock())
            {
                var mappedRoutes = (Dictionary<string, RouteBase>)MappedRoutesField.GetValue(currentRoutes);

                for (var i = 0; i < currentRoutes.Count; i++)
                {
                    var routeBase = currentRoutes[i];
                    var replaceRoute = (System.Web.Routing.RouteBase)null;
                    var mixins = new[] { RouteNameMixin.None() };
                    string routeName = null;

                    if (mappedRoutes.ContainsValue(routeBase))
                    {
                        var pair = mappedRoutes.First(r => r.Value == routeBase);
                        routeName = pair.Key;
                        mixins = new[] { new RouteNameMixin(pair.Key) };
                    }

                    var route = routeBase as System.Web.Routing.Route;
                    if (route != null)
                    {
                        if (routeBase.GetType() == typeof(System.Web.Routing.Route))
                        {
                            replaceRoute = context.ProxyFactory.ExtendClass<System.Web.Routing.Route>(alternateImplementation.AllMethods, mixins, new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                        }
                        else if (context.ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.Route)))
                        {
                            replaceRoute = context.ProxyFactory.WrapClass(route, alternateImplementation.AllMethods, mixins, new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                            RoutesConstraintInspector.Setup(logger, context.ProxyFactory, alternateConstraintImplementation, route.Constraints);
                        }
                    }

                    if (replaceRoute == null)
                    {
                        if (context.ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.RouteBase)))
                        {
                            replaceRoute = context.ProxyFactory.WrapClass(routeBase, alternateBaseImplementation.AllMethods, mixins);
                        }
                    }

                    if (replaceRoute != null)
                    {
                        currentRoutes[i] = replaceRoute;

                        if (routeName != null)
                        {
                            mappedRoutes[routeName] = replaceRoute;
                        }

                        logger.Info(Resources.RouteSetupReplacedRoute, routeBase.GetType());
                    }
                    else
                    {
                        logger.Info(Resources.RouteSetupNotReplacedRoute, routeBase.GetType());
                    } 
                }
            }
        }
    }
}