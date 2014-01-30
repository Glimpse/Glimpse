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
    public class HttpRoute : IAlternateType<System.Web.Http.Routing.IHttpRoute>
    {
        private readonly RouteConstraint routeConstraintAlternate;
        private IEnumerable<IAlternateMethod> allMethodsRouteBase;
        private IEnumerable<IAlternateMethod> allMethodsRoute;

        public HttpRoute(IProxyFactory proxyFactory, ILogger logger)
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
                    new GetRouteData(typeof(System.Web.Http.Routing.IHttpRoute)),
                    new GetVirtualPath(typeof(System.Web.Http.Routing.IHttpRoute))
                });
            }
        }

        public IEnumerable<IAlternateMethod> AllMethodsRoute
        {
            get
            {
                return allMethodsRoute ?? (allMethodsRoute = new List<IAlternateMethod>
                {
                    new GetRouteData(typeof(System.Web.Http.Routing.HttpRoute)),
                    new GetVirtualPath(typeof(System.Web.Http.Routing.HttpRoute)),
                    new ProcessConstraint(),
                });
            }
        }

        private IProxyFactory ProxyFactory { get; set; }

        private ILogger Logger { get; set; }

        public bool TryCreate(System.Web.Http.Routing.IHttpRoute originalObj, out System.Web.Http.Routing.IHttpRoute newObj)
        {
            return TryCreate(originalObj, out newObj, null, null);
        }

        public bool TryCreate(System.Web.Http.Routing.IHttpRoute originalObj, out System.Web.Http.Routing.IHttpRoute newObj, IEnumerable<object> mixins)
        {
            return TryCreate(originalObj, out newObj, mixins, null);
        }

        public bool TryCreate(System.Web.Http.Routing.IHttpRoute originalObj, out System.Web.Http.Routing.IHttpRoute newObj, IEnumerable<object> mixins, object[] constructorArguments)
        {
            newObj = null;

            var route = originalObj as System.Web.Http.Routing.HttpRoute;
            if (route != null)
            {
                if (originalObj.GetType() == typeof(System.Web.Http.Routing.HttpRoute))
                {
                    newObj = ProxyFactory.ExtendClass<System.Web.Http.Routing.HttpRoute>(AllMethodsRoute, mixins, new object[] { route.RouteTemplate, route.Defaults, route.Constraints, route.DataTokens, route.Handler });
                }
                else if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Http.Routing.HttpRoute)))
                {
                    newObj = ProxyFactory.WrapClass(route, AllMethodsRoute, mixins, new object[] { route.RouteTemplate, route.Defaults, route.Constraints, route.DataTokens, route.Handler });
                    SetupConstraints(Logger, ProxyFactory, route.Constraints);
                }
            }

            if (newObj == null)
            {
                if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Http.Routing.IHttpRoute)))
                {
                    newObj = ProxyFactory.WrapClass(originalObj, AllMethodsRouteBase, mixins);
                }
            }

            return newObj != null;
        }

        private void SetupConstraints(ILogger logger, IProxyFactory proxyFactory, IDictionary<string, object> constraints)
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
                    new Message(context.Proxy.GetHashCode(), (System.Web.Http.Routing.HttpRouteData)context.ReturnValue, mixin.Name)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
            }

            public class Message : MessageBase, ITimedMessage, ISourceMessage
            {
                public Message(int routeHashCode, System.Web.Http.Routing.HttpRouteData routeData, string routeName)
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

                public IDictionary<string, object> Values { get; protected set; }

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
                    Values = (System.Web.Http.Routing.HttpRouteValueDictionary)args[1];
                }

                public System.Web.Routing.RequestContext RequestContext { get; private set; }

                public System.Web.Http.Routing.HttpRouteValueDictionary Values { get; private set; }
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
                : base(typeof(System.Web.Http.Routing.HttpRoute), "ProcessConstraint", BindingFlags.NonPublic | BindingFlags.Instance)
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
                    Values = (System.Web.Http.Routing.HttpRouteValueDictionary)args[3];
                    RouteDirection = (System.Web.Http.Routing.HttpRouteDirection)args[4];
                }

                public HttpContextBase HttpContext { get; private set; }

                public object Constraint { get; private set; }

                public string ParameterName { get; private set; }

                public System.Web.Http.Routing.HttpRouteValueDictionary Values { get; private set; }

                public System.Web.Http.Routing.HttpRouteDirection RouteDirection { get; private set; }
            }

            public class Message : ProcessWebApiConstraintMessage
            {
                public Message(Arguments args, int routeHashCode, bool isMatch)
                    : base(routeHashCode, args.Constraint.GetHashCode(), isMatch, args.ParameterName, args.Constraint, args.Values, args.RouteDirection)
                {
                }
            }
        }
    }
}
