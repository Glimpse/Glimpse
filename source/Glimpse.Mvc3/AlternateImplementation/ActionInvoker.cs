using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
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
            yield return new GetFilters<ControllerActionInvoker>();
        }

        public class GetFilters<T> : IAlternateImplementation<T> where T : class
        {
            public GetFilters()
            {
                MethodToImplement = typeof(T).GetMethod("GetFilters", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var timer = context.ProceedWithTimerIfAllowed();
                if (timer == null)
                {
                    return;
                }

                var result = context.ReturnValue as FilterInfo;
                if (result == null)
                {
                    return;
                }

                Proxy(result.ActionFilters, new ActionFilter(context.ProxyFactory));
                Proxy(result.ResultFilters, new ResultFilter(context.ProxyFactory));
                Proxy(result.AuthorizationFilters, new AuthorizationFilter(context.ProxyFactory));
                Proxy(result.ExceptionFilters, new ExceptionFilter(context.ProxyFactory));
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
                var timerResult = context.ProceedWithTimerIfAllowed();

                if (timerResult == null)
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    new Arguments(context.Arguments), 
                    context.MethodInvocationTarget, 
                    timerResult));
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

            public class Message : TimerResultMessage, IExecutionMessage
            {
                public Message(Arguments arguments, MethodInfo method, TimerResult timerResult) : base(timerResult, "ActionResult Executed", "MVC")
                {
                    ActionResultType = arguments.ActionResult.GetType();
                    ExecutedType = arguments.ControllerContext.Controller.GetType();
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    Category = null;
                    ExecutedMethod = method;
                }

                public Type ActionResultType { get; set; }

                public Type ExecutedType { get; private set; }
                
                public bool IsChildAction { get; private set; }
                
                public FilterCategory? Category { get; private set; }

                public MethodInfo ExecutedMethod { get; private set; }
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
                var timerResult = context.ProceedWithTimerIfAllowed();

                if (timerResult == null)
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    new Arguments(context.Arguments), 
                    context.ReturnValue as ActionResult, 
                    context.MethodInvocationTarget, 
                    timerResult));
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

            public class Message : TimerResultMessage, IExecutionMessage
            {
                public Message(Arguments arguments, ActionResult returnValue, MethodInfo method, TimerResult timerResult) : base(timerResult, arguments.ActionDescriptor.ActionName, "MVC")
                {
                    var controllerDescriptor = arguments.ActionDescriptor.ControllerDescriptor;

                    ControllerName = controllerDescriptor.ControllerName;
                    ExecutedType = controllerDescriptor.ControllerType;
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ActionName = arguments.ActionDescriptor.ActionName;
                    ActionResultType = returnValue.GetType();
                    Category = null;
                    ExecutedMethod = method;
                }

                public string ControllerName { get; set; }

                public FilterCategory? Category { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; private set; }

                public bool IsChildAction { get; set; }
                
                public string ActionName { get; set; }
                
                public Type ActionResultType { get; set; }
            }
        }
    }
}