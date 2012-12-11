using System;
using System.Collections.Generic; 
using System.Reflection;
using System.Web.Routing;

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
                    new GetRouteData(typeof(System.Web.Routing.RouteBase)),
                    new GetVirtualPath(typeof(System.Web.Routing.RouteBase))
                }); 
            }
        }

        public class GetRouteData : AlternateMethod
        {
            public GetRouteData(Type type)
                : this(type, "GetRouteData", BindingFlags.Public | BindingFlags.Instance)
            {
            }

            private GetRouteData(Type type, string methodName, BindingFlags bindingFlags)
                : base(type, methodName, bindingFlags)
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(timerResult, context.InvocationTarget.GetType(), context.MethodInvocationTarget, context.Proxy.GetHashCode(), (System.Web.Routing.RouteData)context.ReturnValue));
            }

            public class Message : TimeMessage
            {
                public Message(TimerResult timer, Type executedType, MethodInfo executedMethod, int routeHashCode, System.Web.Routing.RouteData routeData)
                    : base(timer, executedType, executedMethod)
                {
                    IsMatch = routeData != null;
                    RouteHashCode = routeHashCode;

                    if (routeData != null)
                    {
                        Values = routeData.Values;
                    }
                }

                public RouteValueDictionary Values { get; protected set; }

                public int RouteHashCode { get; protected set; }

                public bool IsMatch { get; protected set; }
            }
        } 

        public class GetVirtualPath : AlternateMethod
        {
            public GetVirtualPath(Type type)
                : this(type, "GetVirtualPath", BindingFlags.Public | BindingFlags.Instance)
            {
            }

            private GetVirtualPath(Type type, string methodName, BindingFlags bindingFlags)
                : base(type, methodName, bindingFlags)
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
                    : base(timer, executedType, executedMethod)
                {
                    IsMatch = virtualPathData != null;
                    RouteHashCode = invocationTarget.GetHashCode();
                }

                public int RouteHashCode { get; protected set; }

                public bool IsMatch { get; protected set; }
            }
        }
    }
}
