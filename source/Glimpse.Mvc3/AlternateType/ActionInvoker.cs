using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateType
{
    public class ActionInvoker : AlternateType<IActionInvoker>
    {
        private static MethodInfo executedMethod = typeof(ActionResult).GetMethod("ExecuteResult");
        private IEnumerable<IAlternateMethod> allMethods;

        public ActionInvoker(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new InvokeActionResult<ControllerActionInvoker>(),
                    new InvokeActionMethod(),
                    new GetFilters<ControllerActionInvoker>(new ActionFilter(ProxyFactory), new ResultFilter(ProxyFactory), new AuthorizationFilter(ProxyFactory), new ExceptionFilter(ProxyFactory))
                }); 
            }
        }

        public override bool TryCreate(IActionInvoker originalObj, out IActionInvoker newObj, IEnumerable<object> mixins, object[] constructorArgs)
        {
            if (originalObj == null)
            {
                newObj = null;
                return false;
            }

            var originalType = originalObj.GetType();

            if (originalType == typeof(ControllerActionInvoker) && ProxyFactory.IsExtendClassEligible(originalType))
            {
                newObj = ProxyFactory.ExtendClass<ControllerActionInvoker>(AllMethods);
                return true;
            }

            if (originalObj is ControllerActionInvoker && ProxyFactory.IsWrapClassEligible(originalType))
            {
                newObj = ProxyFactory.WrapClass((ControllerActionInvoker)originalObj, AllMethods);
                return true;
            }

            if (ProxyFactory.IsWrapInterfaceEligible<IActionInvoker>(originalType))
            {
                newObj = ProxyFactory.WrapInterface(originalObj, AllMethods);
                return true;
            }

            newObj = null;
            return false;
        }

        public class GetFilters<T> : AlternateMethod where T : class
        {
            public GetFilters(AlternateType<IActionFilter> alternateActionFilter, AlternateType<IResultFilter> alternateResultFilter, AlternateType<IAuthorizationFilter> alternateAuthorizationFilter, AlternateType<IExceptionFilter> alternateExceptionFilter) : base(typeof(T), "GetFilters", BindingFlags.Instance | BindingFlags.NonPublic)
            {
                AlternateActionFilter = alternateActionFilter;
                AlternateResultFilter = alternateResultFilter;
                AlternateAuthorizationFilter = alternateAuthorizationFilter;
                AlternateExceptionFilter = alternateExceptionFilter;
            }

            public AlternateType<IExceptionFilter> AlternateExceptionFilter { get; set; }

            public AlternateType<IAuthorizationFilter> AlternateAuthorizationFilter { get; set; }

            public AlternateType<IResultFilter> AlternateResultFilter { get; set; }

            public AlternateType<IActionFilter> AlternateActionFilter { get; set; }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var result = context.ReturnValue as FilterInfo;
                if (result == null)
                {
                    return;
                }

                Proxy(result.ActionFilters, AlternateActionFilter);
                Proxy(result.ResultFilters, AlternateResultFilter);
                Proxy(result.AuthorizationFilters, AlternateAuthorizationFilter);
                Proxy(result.ExceptionFilters, AlternateExceptionFilter);
            }

            private void Proxy<TFilter>(IList<TFilter> filters, AlternateType<TFilter> alternateImplementation) where TFilter : class
            {
                var count = filters.Count;

                for (int i = 0; i < count; i++)
                {
                    var originalFilter = filters[i];
                    TFilter newFilter;
                
                    if (alternateImplementation.TryCreate(originalFilter, out newFilter))
                    {
                        filters[i] = newFilter;
                    }
                }
            }
        }

        public class InvokeActionResult<T> : AlternateMethod where T : class
        {
            public InvokeActionResult() : base(typeof(T), "InvokeActionResult", BindingFlags.Instance | BindingFlags.NonPublic)
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(context.Arguments);
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(args.ActionResult.GetType(), executedMethod)
                    .AsChildActionMessage(args.ControllerContext)
                    .AsActionMessage(args.ControllerContext)
                    .AsMvcTimelineMessage(Glimpse.Mvc.Message.Timeline.Controller);

                context.MessageBroker.Publish(message); 
            }

            public class Arguments
            {
                public Arguments(params object[] args)
                {
                    ControllerContext = (ControllerContext)args[0];
                    ActionResult = (ActionResult)args[1];
                }

                public ControllerContext ControllerContext { get; set; }
                
                public ActionResult ActionResult { get; set; }
            }

            public class Message : MessageBase, IExecutionMessage, IInvokeActionResultMessage
            {
                public string ControllerName { get; set; }

                public string ActionName { get; set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }

                public TimeSpan Offset { get; set; }

                public TimeSpan Duration { get; set; }

                public DateTime StartTime { get; set; }

                public string EventName { get; set; }

                public TimelineCategory EventCategory { get; set; }

                public string EventSubText { get; set; }

                public bool IsChildAction { get; set; }
            } 
        }

        public class InvokeActionMethod : AlternateMethod
        {
            public InvokeActionMethod() : base(typeof(ControllerActionInvoker), "InvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic)
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(context.Arguments); 
                var controllerDescriptor = args.ActionDescriptor.ControllerDescriptor; 
                var message = new Message(context.ReturnValue.GetType())
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(controllerDescriptor.ControllerType, controllerDescriptor.ControllerType.GetMethod(args.ActionDescriptor.ActionName))
                    .AsChildActionMessage(args.ControllerContext)
                    .AsActionMessage(args.ControllerContext)
                    .AsMvcTimelineMessage(Glimpse.Mvc.Message.Timeline.Controller);

                context.MessageBroker.Publish(message);  
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    ControllerContext = (ControllerContext)arguments[0];
                    ActionDescriptor = (ActionDescriptor)arguments[1];
                    Parameters = (IDictionary<string, object>)arguments[2];

                    if (arguments.Length == 5)
                    {
                        IsAsync = true;
                        Callback = (AsyncCallback)arguments[3];
                        State = arguments[4];
                    }
                }

                public ControllerContext ControllerContext { get; set; }
                
                public ActionDescriptor ActionDescriptor { get; set; }
                
                public IDictionary<string, object> Parameters { get; set; }
                
                public AsyncCallback Callback { get; set; }
                
                public object State { get; set; }
                
                public bool IsAsync { get; set; }
            }

            public class Message : MessageBase, IExecutionMessage
            {
                public Message(Type resultType)
                {
                    ResultType = resultType;
                }

                public string ControllerName { get; set; }

                public string ActionName { get; set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }

                public TimeSpan Offset { get; set; }

                public TimeSpan Duration { get; set; }

                public DateTime StartTime { get; set; }

                public string EventName { get; set; }

                public TimelineCategory EventCategory { get; set; }

                public string EventSubText { get; set; }

                public Type ResultType { get; private set; }

                public bool IsChildAction { get; set; }
            } 
        }
    }
}