using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet.AlternateImplementation
{
    /// <summary>
    /// Generator of Route proxies. The proxy intercepts Route.ProcessConstraint and records
    /// the exection details.
    /// </summary>
    public static class Route
    {
        public static IEnumerable<IAlternateImplementation<System.Web.Routing.Route>> AllMethods(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            yield return new ProcessConstraint(messageBroker, timerStrategy, runtimePolicyStrategy);
        }

        /// <summary>
        /// protected virtual bool ProcessConstraint(HttpContextBase httpContext,
        ///     object constraint, string parameterName, RouteValueDictionary values,
        ///     RouteDirection routeDirection)
        /// </summary>
        public class ProcessConstraint : IAlternateImplementation<System.Web.Routing.Route>
        {
            public ProcessConstraint(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
            {
                MessageBroker = messageBroker;
                TimerStrategy = timerStrategy;
                RuntimePolicyStrategy = runtimePolicyStrategy;
            }

            public IMessageBroker MessageBroker { get; set; }
            public Func<IExecutionTimer> TimerStrategy { get; set; }
            public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

            public MethodInfo MethodToImplement
            {
                get { return typeof (System.Web.Routing.Route).GetMethod("ProcessConstraint", BindingFlags.NonPublic | BindingFlags.Instance); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                context.Proceed();

                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    return;
                }

                var args = new Arguments(context.Arguments);
                var result = (bool)context.ReturnValue;

                var msg = new Message((System.Web.Routing.Route)context.InvocationTarget, result, args);
                MessageBroker.Publish(msg);
            }

            public class Arguments
            {
                public Arguments(object [] args)
                {
                    HttpContext = (HttpContextBase) args[0];
                    Constraint = args[1];
                    ParameterName = (string) args[2];
                    Values = (RouteValueDictionary) args[3];
                    RouteDirection = (RouteDirection) args[4];
                }

                public HttpContextBase HttpContext { get; set; }
                public object Constraint { get; set; }
                public string ParameterName { get; set; }
                public RouteValueDictionary Values { get; set; }
                public RouteDirection RouteDirection { get; set; }
            }

            public class Message
            {
                public bool IsMatch { get; set; }
                public System.Web.Routing.Route Route { get; set; }
                public string ParameterName { get; set; }

                public Message(System.Web.Routing.Route route, bool isMatch, Arguments args)
                {
                    IsMatch = isMatch;
                    Route = route;
                    ParameterName = args.ParameterName;
                }
            }
        }
    }
}
