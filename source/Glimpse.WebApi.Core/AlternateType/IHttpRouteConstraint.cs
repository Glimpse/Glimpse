using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Glimpse.WebApi.Core.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.WebApi.Core.AlternateType
{
    public class IHttpRouteConstraint : AlternateType<System.Web.Http.Routing.IHttpRouteConstraint>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public IHttpRouteConstraint(IProxyFactory proxyFactory)
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
                : base(typeof(System.Web.Http.Routing.IHttpRouteConstraint), "Match", BindingFlags.Public | BindingFlags.Instance)
            { 
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(
                    new Message(new Arguments(context.Arguments), context.InvocationTarget, (bool)context.ReturnValue)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)); 
            }

            public class Arguments
            {
                public Arguments(object[] args)
                {
                    HttpContext = (HttpContextBase)args[0];
                    Route = (System.Web.Http.Routing.HttpRoute)args[1];
                    ParameterName = (string)args[2];
                    Values = (System.Web.Http.Routing.HttpRouteValueDictionary)args[3];
                    RouteDirection = (System.Web.Http.Routing.HttpRouteDirection)args[4];
                }

                public HttpContextBase HttpContext { get; private set; }

                public System.Web.Http.Routing.HttpRoute Route { get; private set; }

                public string ParameterName { get; private set; }

                public System.Web.Http.Routing.HttpRouteValueDictionary Values { get; private set; }

                public System.Web.Http.Routing.HttpRouteDirection RouteDirection { get; private set; }
            }

            public class Message : ProcessConstraintMessage
            {
                public Message(Arguments args, object invocationTarget, bool isMatch) 
                    : base(args.Route.GetHashCode(), invocationTarget.GetHashCode(), isMatch, args.ParameterName, invocationTarget, args.Values, args.RouteDirection)
                { 
                } 
            }
        }
    }
}
