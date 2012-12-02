using System;
using System.Collections.Generic; 
using System.Reflection; 
using System.Web;
using System.Web.Routing; 
using Glimpse.AspNet.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions; 

namespace Glimpse.AspNet.AlternateImplementation
{
    public class Route : AlternateType<System.Web.Routing.Route>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public Route(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new ProcessConstraint(),
                        new RouteBase.GetRouteData<System.Web.Routing.Route>(),
                        new RouteBase.GetVirtualPath<System.Web.Routing.Route>()
                    });
            }
        }

        public class ProcessConstraint : AlternateMethod
        {
            public ProcessConstraint()
                : base(typeof(System.Web.Routing.Route), "ProcessConstraint", BindingFlags.NonPublic | BindingFlags.Instance)
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), timerResult, context.InvocationTarget.GetType(), context.MethodInvocationTarget, context.InvocationTarget.GetHashCode(), (bool)context.ReturnValue));
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

                public HttpContextBase HttpContext { get; private set; }

                public object Constraint { get; private set; }

                public string ParameterName { get; private set; }

                public RouteValueDictionary Values { get; private set; }

                public RouteDirection RouteDirection { get; private set; }
            }

            public class Message : ProcessConstraintMessage
            {
                public Message(Arguments args, TimerResult timer, Type executedType, MethodInfo executedMethod, int routeHashCode, bool isMatch)
                    : base(timer, executedType, executedMethod, routeHashCode, isMatch, args.ParameterName, args.Constraint, args.Values)
                { 
                } 
            }
        }
    }
}
