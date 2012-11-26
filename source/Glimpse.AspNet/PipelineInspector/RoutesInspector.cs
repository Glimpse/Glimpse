using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{ 
    public class RoutesInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;
            var alternateImplementation = new Glimpse.AspNet.AlternateImplementation.Route(context.ProxyFactory);

            var currentRoutes = System.Web.Routing.RouteTable.Routes;
            using (currentRoutes.GetWriteLock())
            {
                for (var i = 0; i < currentRoutes.Count; i++)
                {
                    var originalRoute = currentRoutes[i] as System.Web.Routing.Route;
                    if (originalRoute == null)
                    {
                        continue;
                    }

                    System.Web.Routing.Route newRoute;
                    if (alternateImplementation.TryCreate(originalRoute, out newRoute, constructorArguments: new object[] { originalRoute.Url, originalRoute.Defaults, originalRoute.Constraints, originalRoute.DataTokens, originalRoute.RouteHandler }))
                    {
                        currentRoutes[i] = newRoute;
                        logger.Info(Resources.RouteSetupReplacedRoute, originalRoute.GetType());
                    }
                }
            }
        }
    }
}