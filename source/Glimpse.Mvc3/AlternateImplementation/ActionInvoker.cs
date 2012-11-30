using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ActionInvoker : AlternateType<ControllerActionInvoker>
    {
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

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
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

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                // void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), timerResult));
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

            public class Message : ActionMessage
            {
                private static MethodInfo executedMethod = typeof(ActionResult).GetMethod("ExecuteResult");

                public Message(Arguments arguments, TimerResult timerResult)
                    : base(timerResult, GetControllerName(arguments.ControllerContext), GetActionName(arguments.ControllerContext), GetIsChildAction(arguments.ControllerContext), arguments.ActionResult.GetType(), executedMethod)
                {
                    EventName = string.Format("InvokeActionResult - {0}:{1}", ControllerName, ActionName);
                    EventCategory = "Controller";
                }
            }
        }

        public class InvokeActionMethod : AlternateMethod
        {
            public InvokeActionMethod() : base(typeof(ControllerActionInvoker), "InvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic)
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                //// ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.ReturnValue as ActionResult, context.MethodInvocationTarget, timerResult));
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

            public class Message : ActionMessage
            {
                public Message(Arguments arguments, ActionResult returnValue, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(arguments.ActionDescriptor), GetActionName(arguments.ActionDescriptor), GetIsChildAction(arguments.ControllerContext), GetExecutedType(arguments.ActionDescriptor), method)
                {
                    ResultType = returnValue.GetType();
                    EventName = string.Format("InvokeActionMethod - {0}:{1}", ControllerName, ActionName);
                    EventCategory = "Controller";
                }

                public Type ResultType { get; set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("ResultType", ResultType);
                }
            }
        }
    }
}