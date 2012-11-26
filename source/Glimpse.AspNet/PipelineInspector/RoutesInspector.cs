using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{ 
    public class RoutesInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;
            var alternateImplementation = new Glimpse.AspNet.AlternateImplementation.Route(context.ProxyFactory);
            var alternateBaseImplementation = new Glimpse.AspNet.AlternateImplementation.RouteBase(context.ProxyFactory);

            var currentRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentRoutes.GetWriteLock())
            {
                for (var i = 0; i < currentRoutes.Count; i++)
                {
                    var replaceRoute = (System.Web.Routing.RouteBase)null; 

                    var routeBase = currentRoutes[i]; 
                    var route = routeBase as System.Web.Routing.Route;
                    if (route != null)
                    {
                        System.Web.Routing.Route newRoute;
                        if (alternateImplementation.TryCreate(route, out newRoute, constructorArguments: new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler }))
                        {
                            replaceRoute = newRoute; 
                        }
                    }
                    else
                    {
                        System.Web.Routing.RouteBase newRouteBase;
                        if (alternateBaseImplementation.TryCreate(routeBase, out newRouteBase))
                        {
                            replaceRoute = newRouteBase;
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