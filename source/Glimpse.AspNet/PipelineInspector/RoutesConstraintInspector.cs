using System.Linq;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.PipelineInspector
{
    public class RoutesConstraintInspector
    {
        public void Setup(ILogger logger, IProxyFactory proxyFactory, RouteConstraint alternateConstraintImplementation, System.Web.Routing.RouteValueDictionary constraints)
        {
            if (constraints != null)
            {
                var keys = constraints.Keys.ToList();
                for (var i = 0; i < keys.Count; i++)
                {
                    var constraintKey = keys[i];
                    var constraint = constraints[constraintKey];

                    var routeConstraint = constraint as System.Web.Routing.IRouteConstraint;
                    if (routeConstraint == null)
                    {
                        var stringRouteConstraint = constraint as string;
                        if (stringRouteConstraint != null)
                        {
                            routeConstraint = new RouteConstraintRegex(stringRouteConstraint);
                        }
                    }

                    if (routeConstraint != null)
                    {
                        var replaceRouteConstraint = proxyFactory.WrapInterface(routeConstraint, alternateConstraintImplementation.AllMethods);
                        if (replaceRouteConstraint != null)
                        {
                            constraints[constraintKey] = replaceRouteConstraint;
                            logger.Info(Resources.RouteSetupReplacedRoute, constraint.GetType());
                        }
                        else
                        {
                            logger.Info(Resources.RouteSetupNotReplacedRoute, constraint.GetType());
                        }
                    }
                }
            }
        }
    }
}