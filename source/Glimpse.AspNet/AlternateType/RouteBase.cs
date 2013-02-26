using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web; 
using Glimpse.AspNet.Message; 
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.AspNet.AlternateType
{
    public class RouteBase : IAlternateType<System.Web.Routing.RouteBase>
    {
        private readonly RouteConstraint routeConstraintAlternate;
        private IEnumerable<IAlternateMethod> allMethodsRouteBase;
        private IEnumerable<IAlternateMethod> allMethodsRoute;

        public RouteBase(IProxyFactory proxyFactory, ILogger logger)
        {
            ProxyFactory = proxyFactory;
            Logger = logger;
            routeConstraintAlternate = new RouteConstraint(proxyFactory);
        }

        public IEnumerable<IAlternateMethod> AllMethodsRouteBase
        {
            get
            {
                return allMethodsRouteBase ?? (allMethodsRouteBase = new List<IAlternateMethod>
                {
                    new GetRouteData(typeof(System.Web.Routing.RouteBase)),
                    new GetVirtualPath(typeof(System.Web.Routing.RouteBase))
                });
            }
        }

        public IEnumerable<IAlternateMethod> AllMethodsRoute
        {
            get
            {
                return allMethodsRoute ?? (allMethodsRoute = new List<IAlternateMethod>
                {
                    new GetRouteData(typeof(System.Web.Routing.Route)),
                    new GetVirtualPath(typeof(System.Web.Routing.Route)),
                    new ProcessConstraint(),
                });
            }
        }

        private IProxyFactory ProxyFactory { get; set; }

        private ILogger Logger { get; set; }

        public bool TryCreate(System.Web.Routing.RouteBase originalObj, out System.Web.Routing.RouteBase newObj)
        {
            return TryCreate(originalObj, out newObj, null, null);
        }

        public bool TryCreate(System.Web.Routing.RouteBase originalObj, out System.Web.Routing.RouteBase newObj, IEnumerable<object> mixins)
        {
            return TryCreate(originalObj, out newObj, mixins, null);
        }

        public bool TryCreate(System.Web.Routing.RouteBase originalObj, out System.Web.Routing.RouteBase newObj, IEnumerable<object> mixins, object[] constructorArguments)
        {
            newObj = null;

            var route = originalObj as System.Web.Routing.Route;
            if (route != null)
            {
                if (originalObj.GetType() == typeof(System.Web.Routing.Route))
                {
                    newObj = ProxyFactory.ExtendClass<System.Web.Routing.Route>(AllMethodsRoute, mixins, new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                }
                else if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.Route)))
                {
                    newObj = ProxyFactory.WrapClass(route, AllMethodsRoute, mixins, new object[] { route.Url, route.Defaults, route.Constraints, route.DataTokens, route.RouteHandler });
                    SetupConstraints(Logger, ProxyFactory, route.Constraints);
                }
            }

            if (newObj == null)
            {
                if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Routing.RouteBase)))
                {
                    newObj = ProxyFactory.WrapClass(originalObj, AllMethodsRouteBase, mixins);
                }
            }

            return newObj != null;
        }
         
        private void SetupConstraints(ILogger logger, IProxyFactory proxyFactory, System.Web.Routing.RouteValueDictionary constraints)
        {
            if (constraints != null)
            {
                var keys = constraints.Keys.ToList();
                for (var i = 0; i < keys.Count; i++)
                {
                    var constraintKey = keys[i];
                    var constraint = constraints[constraintKey];

                    var originalObj = constraint as System.Web.Routing.IRouteConstraint;
                    var newObj = (System.Web.Routing.IRouteConstraint)null;
                    if (originalObj == null)
                    {
                        var stringRouteConstraint = constraint as string;
                        if (stringRouteConstraint != null)
                        {
                            newObj = new RouteConstraintRegex(stringRouteConstraint);
                        }
                    }
                    else
                    {
                        routeConstraintAlternate.TryCreate(originalObj, out newObj);
                    }

                    if (newObj != null)
                    {
                        constraints[constraintKey] = newObj;
                        logger.Info(Resources.RouteSetupReplacedRoute, constraint.GetType());
                    }
                    else
                    {
                        logger.Info(Resources.RouteSetupNotReplacedRoute, constraint.GetType());
                    } 
                }
            }
        }
         
        public class GetRouteData : AlternateMethod
        {
            public GetRouteData(Type type)
                : base(type, "GetRouteData", BindingFlags.Public | BindingFlags.Instance)
            {
            } 

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var mixin = (IRouteNameMixin)context.Proxy;

                context.MessageBroker.Publish(
                    new Message(context.Proxy.GetHashCode(), (System.Web.Routing.RouteData)context.ReturnValue, mixin.Name)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
            }

            public class Message : MessageBase, ITimedMessage, ISourceMessage
            {
                public Message(int routeHashCode, System.Web.Routing.RouteData routeData, string routeName) 
                {
                    IsMatch = routeData != null;
                    RouteHashCode = routeHashCode;
                    RouteName = routeName;

                    if (routeData != null)
                    {
                        Values = routeData.Values;
                    }
                }
                 
                public TimeSpan Offset { get; set; }

                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }

                public Type ExecutedType { get; set; }
                
                public MethodInfo ExecutedMethod { get; set; }
                
                public System.Web.Routing.RouteValueDictionary Values { get; protected set; } 
                
                public int RouteHashCode { get; protected set; } 
                
                public bool IsMatch { get; protected set; } 
                
                public string RouteName { get; protected set; }
            }
        } 

        public class GetVirtualPath : AlternateMethod
        {
            public GetVirtualPath(Type type)
                : base(type, "GetVirtualPath", BindingFlags.Public | BindingFlags.Instance)
            {
            } 

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(
                    new Arguments(context.Arguments), context.InvocationTarget, (System.Web.Routing.VirtualPathData)context.ReturnValue)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
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

            public class Message : ITimedMessage, ISourceMessage
            {
                public Message(Arguments args, object invocationTarget, System.Web.Routing.VirtualPathData virtualPathData) 
                {
                    IsMatch = virtualPathData != null;
                    RouteHashCode = invocationTarget.GetHashCode();
                }

                public int RouteHashCode { get; protected set; } 

                public bool IsMatch { get; protected set; }
                
                public Guid Id { get; private set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public Type ExecutedType { get; set; }
                
                public MethodInfo ExecutedMethod { get; set; }
            }
        }

        public class ProcessConstraint : AlternateMethod
        {
            public ProcessConstraint()
                : base(typeof(System.Web.Routing.Route), "ProcessConstraint", BindingFlags.NonPublic | BindingFlags.Instance)
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(
                    new Message(new Arguments(context.Arguments), context.InvocationTarget.GetHashCode(), (bool)context.ReturnValue)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
            }

            public class Arguments
            {
                public Arguments(object[] args)
                {
                    HttpContext = (HttpContextBase)args[0];
                    Constraint = args[1];
                    ParameterName = (string)args[2];
                    Values = (System.Web.Routing.RouteValueDictionary)args[3];
                    RouteDirection = (System.Web.Routing.RouteDirection)args[4];
                }

                public HttpContextBase HttpContext { get; private set; }

                public object Constraint { get; private set; }

                public string ParameterName { get; private set; }

                public System.Web.Routing.RouteValueDictionary Values { get; private set; }

                public System.Web.Routing.RouteDirection RouteDirection { get; private set; }
            }

            public class Message : ProcessConstraintMessage
            {
                public Message(Arguments args, int routeHashCode, bool isMatch)
                    : base(routeHashCode, args.Constraint.GetHashCode(), isMatch, args.ParameterName, args.Constraint, args.Values, args.RouteDirection) 
                { 
                } 
            }
        }
    }
}
