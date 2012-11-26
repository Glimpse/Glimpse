using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.AspNet.AlternateImplementation
{
    public class Route : Alternate<System.Web.Routing.Route>
    {
        public Route(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<System.Web.Routing.Route>> AllMethods()
        {
            yield return new ProcessConstraint();
            yield return new RouteBase.GetRouteData<System.Web.Routing.Route>();
        }

        public class ProcessConstraint : IAlternateImplementation<System.Web.Routing.Route>
        {
            public ProcessConstraint()
            {
                MethodToImplement = typeof(System.Web.Routing.Route).GetMethod("ProcessConstraint", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            public MethodInfo MethodToImplement { get; set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                var result = (bool)context.ReturnValue;

                context.MessageBroker.Publish(new Message((System.Web.Routing.Route)context.InvocationTarget, new Arguments(context.Arguments), result));
            }

            public class Arguments
            {
                public Arguments(object[] args)
                {
                    HttpContext = (HttpContextBase)args[0];
                    Constraint = args[1];
                    ParameterName = (string)args[2];
                    Values = (RouteValueDictionary)args[3];
                    RouteDirection = (RouteDirection)args[4];
                }

                public HttpContextBase HttpContext { get; set; }

                public object Constraint { get; set; }

                public string ParameterName { get; set; }

                public RouteValueDictionary Values { get; set; }

                public RouteDirection RouteDirection { get; set; }
            }

            public class Message
            {
                public Message(System.Web.Routing.Route route, Arguments args, bool isMatch)
                {
                    IsMatch = isMatch;
                    ParameterName = args.ParameterName;
                    Route = route;
                }

                public bool IsMatch { get; set; }

                public System.Web.Routing.Route Route { get; set; }

                public string ParameterName { get; set; }
            }
        }
    }
}
