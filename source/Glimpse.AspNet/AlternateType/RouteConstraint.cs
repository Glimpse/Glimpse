using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Glimpse.AspNet.Message;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.AlternateType
{
    public class RouteConstraint : AlternateType<System.Web.Routing.IRouteConstraint>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public RouteConstraint(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new Match()
                    }); 
            }
        }

        public class Match : AlternateMethod
        {
            public Match()
                : base(typeof(System.Web.Routing.IRouteConstraint), "Match", BindingFlags.Public | BindingFlags.Instance)
            { 
            }
             
            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), timerResult, context.InvocationTarget.GetType(), context.MethodInvocationTarget, context.InvocationTarget, (bool)context.ReturnValue));
            }

            public class Arguments
            {
                public Arguments(object[] args)
                {
                    HttpContext = (HttpContextBase)args[0];
                    Route = (System.Web.Routing.Route)args[1];
                    ParameterName = (string)args[2];
                    Values = (System.Web.Routing.RouteValueDictionary)args[3];
                    RouteDirection = (System.Web.Routing.RouteDirection)args[4];
                }

                public HttpContextBase HttpContext { get; private set; }

                public System.Web.Routing.Route Route { get; private set; }

                public string ParameterName { get; private set; }

                public System.Web.Routing.RouteValueDictionary Values { get; private set; }

                public System.Web.Routing.RouteDirection RouteDirection { get; private set; }
            }

            public class Message : ProcessConstraintMessage
            {
                public Message(Arguments args, TimerResult timer, Type executedType, MethodInfo executedMethod, object invocationTarget, bool isMatch)
                    : base(timer, executedType, executedMethod, args.Route.GetHashCode(), invocationTarget.GetHashCode(), isMatch, args.ParameterName, executedType, args.Values, args.RouteDirection)
                { 
                } 
            }
        }
    }
}
