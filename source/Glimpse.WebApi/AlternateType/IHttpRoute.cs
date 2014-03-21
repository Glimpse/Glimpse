using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Glimpse.WebApi.Message;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.WebApi.AlternateType
{
    public class IHttpRoute : IAlternateType<System.Web.Http.Routing.IHttpRoute>
    {
        private readonly IHttpRouteConstraint routeConstraintAlternate;
        private IEnumerable<IAlternateMethod> allMethodsIHttpRoute;
        private IEnumerable<IAlternateMethod> allMethodsHttpRoute;

        public IHttpRoute(IProxyFactory proxyFactory, ILogger logger)
        {
            ProxyFactory = proxyFactory;
            Logger = logger;
            routeConstraintAlternate = new IHttpRouteConstraint(proxyFactory);
        }

        public IEnumerable<IAlternateMethod> AllMethodsIHttpRoute
        {
            get
            {
                return allMethodsIHttpRoute ?? (allMethodsIHttpRoute = new List<IAlternateMethod>
                {
                    new GetRouteData(typeof(System.Web.Http.Routing.IHttpRoute)),
                    new GetVirtualPath(typeof(System.Web.Http.Routing.IHttpRoute))
                });
            }
        }

        public IEnumerable<IAlternateMethod> AllMethodsHttpRoute
        {
            get
            {
                return allMethodsHttpRoute ?? (allMethodsHttpRoute = new List<IAlternateMethod>
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
                    newObj = ProxyFactory.ExtendClass<System.Web.Http.Routing.HttpRoute>(AllMethodsHttpRoute, mixins, new object[] { route.RouteTemplate, route.Defaults, route.Constraints, route.DataTokens, route.Handler });
                }
                else if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Http.Routing.HttpRoute)))
                {
                    newObj = ProxyFactory.WrapClass(route, AllMethodsHttpRoute, mixins, new object[] { route.RouteTemplate, route.Defaults, route.Constraints, route.DataTokens, route.Handler });
                    SetupConstraints(Logger, ProxyFactory, route.Constraints);
                }
            }

            if (newObj == null)
            {
                if (ProxyFactory.IsWrapClassEligible(typeof(System.Web.Http.Routing.IHttpRoute)))
                {
                    newObj = ProxyFactory.WrapClass(originalObj, AllMethodsIHttpRoute, mixins);
                }
                else if (ProxyFactory.IsWrapInterfaceEligible<System.Web.Http.Routing.IHttpRoute>(typeof(System.Web.Http.Routing.IHttpRoute)))
                {
                    newObj = ProxyFactory.WrapInterface(originalObj, AllMethodsIHttpRoute, mixins);
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

                    var originalObj = constraint as System.Web.Http.Routing.IHttpRouteConstraint;
                    var newObj = (System.Web.Http.Routing.IHttpRouteConstraint)null;
                    if (originalObj == null)
                    {
                        var stringRouteConstraint = constraint as string;
                        if (stringRouteConstraint != null)
                        {
                            newObj = new IHttpRouteConstraintRegex(stringRouteConstraint);
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
                    new Message(context.Proxy.GetHashCode(), (System.Web.Http.Routing.IHttpRouteData)context.ReturnValue, mixin.Name)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
            }

            public class Message : MessageBase, ITimedMessage, ISourceMessage
            {
                public Message(int routeHashCode, System.Web.Http.Routing.IHttpRouteData routeData, string routeName)
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
                    new Arguments(context.Arguments), context.InvocationTarget, (System.Web.Http.Routing.IHttpVirtualPathData)context.ReturnValue)
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget));
            }

            public class Arguments
            {
                public Arguments(params object[] args)
                {
                    Request = (System.Net.Http.HttpRequestMessage)args[0];
                    Values = (System.Web.Http.Routing.HttpRouteValueDictionary)args[1];
                }

                public System.Net.Http.HttpRequestMessage Request { get; private set; }

                public System.Web.Http.Routing.HttpRouteValueDictionary Values { get; private set; }
            }

            public class Message : ITimedMessage, ISourceMessage
            {
                public Message(Arguments args, object invocationTarget, System.Web.Http.Routing.IHttpVirtualPathData virtualPathData)
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
                    Request = (System.Net.Http.HttpRequestMessage)args[0];
                    Constraint = args[1];
                    ParameterName = (string)args[2];
                    Values = (System.Web.Http.Routing.HttpRouteValueDictionary)args[3];
                    RouteDirection = (System.Web.Http.Routing.HttpRouteDirection)args[4];
                }

                public System.Net.Http.HttpRequestMessage Request { get; private set; }

                public object Constraint { get; private set; }

                public string ParameterName { get; private set; }

                public System.Web.Http.Routing.HttpRouteValueDictionary Values { get; private set; }

                public System.Web.Http.Routing.HttpRouteDirection RouteDirection { get; private set; }
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
