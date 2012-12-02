using System;
using System.Collections.Generic; 
using System.Reflection; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.AlternateImplementation
{
    public class RouteBase : AlternateType<System.Web.Routing.RouteBase>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public RouteBase(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new GetRouteData<System.Web.Routing.RouteBase>(),
                    new GetVirtualPath<System.Web.Routing.RouteBase>()
                }); 
            }
        }

        public class GetRouteData<T> : AlternateMethod
            where T : System.Web.Routing.RouteBase
        {
            public GetRouteData()
                : base(typeof(T), "GetRouteData", BindingFlags.Public | BindingFlags.Instance)
            {   
            }
             
            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(timerResult, context.InvocationTarget.GetType(), context.MethodInvocationTarget, context.InvocationTarget, (System.Web.Routing.RouteData)context.ReturnValue));
            }

            public class Message : TimeMessage
            {
                public Message(TimerResult timer, Type executedType, MethodInfo executedMethod, object invocationTarget, System.Web.Routing.RouteData routeData)
                    : base(timer)
                {
                    ExecutedType = executedType;
                    ExecutedMethod = executedMethod;
                    IsMatch = routeData != null;
                    RouteHashCode = invocationTarget.GetHashCode();
                }

                public MethodInfo ExecutedMethod { get; protected set; }

                public Type ExecutedType { get; protected set; }
                 
                public int RouteHashCode { get; protected set; }

                public bool IsMatch { get; protected set; }
            }
        }

        public class GetVirtualPath<T> : AlternateMethod
            where T : System.Web.Routing.RouteBase
        {
            public GetVirtualPath()
                : base(typeof(T), "GetVirtualPath", BindingFlags.Public | BindingFlags.Instance)
            { 
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), timerResult, context.InvocationTarget.GetType(), context.MethodInvocationTarget, context.InvocationTarget, (System.Web.Routing.VirtualPathData)context.ReturnValue));
            }

            public class Arguments
            {
                public Arguments(params object[] args)
                {
                    RequestContext = (System.Web.Routing.RequestContext)args[0];
                    Values = (System.Web.Routing.RouteValueDictionary)args[1];
                }

                public System.Web.Routing.RequestContext RequestContext { get; private set; }

                public System.Web.Routing.RouteValueDictionary Values { get; private set; }
            }

            public class Message : TimeMessage
            {
                public Message(Arguments args, TimerResult timer, Type executedType, MethodInfo executedMethod, object invocationTarget, System.Web.Routing.VirtualPathData virtualPathData)
                    : base(timer)
                {
                    ExecutedType = executedType;
                    ExecutedMethod = executedMethod; 
                    IsMatch = virtualPathData != null;
                    RouteHashCode = invocationTarget.GetHashCode();
                }

                public MethodInfo ExecutedMethod { get; private set; }

                public Type ExecutedType { get; private set; }

                public int RouteHashCode { get; protected set; } 

                public bool IsMatch { get; protected set; }
            }
        }
    }
}
