using System.Web.Routing;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{
    /// <summary>
    /// PipelineInspector which will replace any routes in the route table
    /// with a proxied version (from Glimpse.AspNet.AlternateImplementation.Route)
    /// </summary>
    public class RoutesInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
           using (RouteTable.Routes.GetWriteLock())
           {
               for (var i = 0; i < RouteTable.Routes.Count; i++)
               {
                   var route = RouteTable.Routes[i] as Route;
                   if (route == null)
                   {
                       // we don't try to do any magic with anything which doesn't implement Route.
                       continue;
                   }

                   if (!context.ProxyFactory.IsProxyable(route))
                   {
                       // nor anything which isn't proxyable.
                       continue;
                   }

                   var methods = AlternateImplementation.Route.AllMethods(context.MessageBroker,
                                                                          context.TimerStrategy,
                                                                          context.RuntimePolicyStrategy);

                   var newRoute = context.ProxyFactory.CreateProxy(route, methods);

                   RouteTable.Routes[i] = newRoute;
               }
           }
        }
    }
}