using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ActionInvoker : Alternate<ControllerActionInvoker>
    {
        public ActionInvoker(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<ControllerActionInvoker>> AllMethods()
        {
            yield return new InvokeActionResult<ControllerActionInvoker>();
            yield return new InvokeActionMethod();
            yield return new GetFilters<ControllerActionInvoker>(new ActionFilter(ProxyFactory), new ResultFilter(ProxyFactory), new AuthorizationFilter(ProxyFactory), new ExceptionFilter(ProxyFactory));
        }

        public class GetFilters<T> : IAlternateImplementation<T> where T : class
        {
            public GetFilters(Alternate<IActionFilter> alternateActionFilter, Alternate<IResultFilter> alternateResultFilter, Alternate<IAuthorizationFilter> alternateAuthorizationFilter, Alternate<IExceptionFilter> alternateExceptionFilter)
            {
                AlternateActionFilter = alternateActionFilter;
                AlternateResultFilter = alternateResultFilter;
                AlternateAuthorizationFilter = alternateAuthorizationFilter;
                AlternateExceptionFilter = alternateExceptionFilter;
                MethodToImplement = typeof(T).GetMethod("GetFilters", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public Alternate<IExceptionFilter> AlternateExceptionFilter { get; set; }

            public Alternate<IAuthorizationFilter> AlternateAuthorizationFilter { get; set; }

            public Alternate<IResultFilter> AlternateResultFilter { get; set; }

            public Alternate<IActionFilter> AlternateActionFilter { get; set; }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

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

            private void Proxy<TFilter>(IList<TFilter> filters, Alternate<TFilter> alternateImplementation) where TFilter : class
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

        public class InvokeActionResult<T> : IAlternateImplementation<T> where T : class
        {
            public MethodInfo MethodToImplement
            {
                get { return typeof(T).GetMethod("InvokeActionResult", BindingFlags.Instance | BindingFlags.NonPublic); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                // void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), timerResult));
            }

            public class Arguments
            {
                public Arguments(object[] args)
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

        public class InvokeActionMethod : IAlternateImplementation<ControllerActionInvoker>
        {
            public MethodInfo MethodToImplement
            {
                get { return typeof(ControllerActionInvoker).GetMethod("InvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                //// ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.ReturnValue as ActionResult, context.MethodInvocationTarget, timerResult));
            }

            public class Arguments
            {
                public Arguments(object[] arguments)
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