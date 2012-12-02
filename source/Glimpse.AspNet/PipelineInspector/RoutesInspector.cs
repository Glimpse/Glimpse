using System.Linq; 
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{
    public class RoutesInspector : IPipelineInspector
    {
        public RoutesInspector()
        {
            RoutesConstraintInspector = new RoutesConstraintInspector();
        }

        private RoutesConstraintInspector RoutesConstraintInspector { get; set; }

        public void Setup(IPipelineInspectorContext context)
        { 
            var logger = context.Logger; 
            var alternateImplementation = new Glimpse.AspNet.AlternateImplementation.Route(context.ProxyFactory);
            var alternateBaseImplementation = new Glimpse.AspNet.AlternateImplementation.RouteBase(context.ProxyFactory);
            var alternateConstraintImplementation = new Glimpse.AspNet.AlternateImplementation.RouteConstraint(context.ProxyFactory); 

            var currentRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentRoutes.GetWriteLock())
            {
                for (var i = 0; i < currentRoutes.Count; i++)
                {
                    var routeBase = currentRoutes[i];
                    var replaceRoute = (System.Web.Routing.RouteBase)null;

                    var route = routeBase as System.Web.Routing.Route;
                    if (route != null)
                    {
                        if (routeBase.GetType() == typeof(System.Web.Routing.Route))
                        {
                            replaceRoute = context.ProxyFactory.ExtendClass<System.Web.Routing.Route>(alternateImplementation.AllMethods, Enumerable.Empty<object>(), new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                        }
                        else if (context.ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.Route)))
                        {
                            replaceRoute = context.ProxyFactory.WrapClass(route, alternateImplementation.AllMethods, Enumerable.Empty<object>(), new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                            RoutesConstraintInspector.Setup(logger, context.ProxyFactory, alternateConstraintImplementation, route.Constraints);
                        }
                    }

                    if (replaceRoute == null)
                    {
                        if (context.ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.RouteBase)))
                        {
                            replaceRoute = context.ProxyFactory.WrapClass(routeBase, alternateBaseImplementation.AllMethods);
                        }
                    }

                    if (replaceRoute != null)
                    {
                        currentRoutes[i] = replaceRoute;
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