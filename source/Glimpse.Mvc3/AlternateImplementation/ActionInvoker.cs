using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
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
                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var timer = context.TimerStrategy();
                var timerResult = timer.Time(context.Proceed);

                context.MessageBroker.Publish(new TimerResultMessage(timerResult, "ActionResult Executed", "MVC")); // TODO clean this up, use a constant?
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments)));
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

            public class Message
            {
                public Message(Arguments arguments)
                {
                    ActionResultType = arguments.ActionResult.GetType();
                    ConrollerType = arguments.ControllerContext.Controller.GetType();
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                }

                public Type ActionResultType { get; set; }
                
                public Type ConrollerType { get; set; }
                
                public bool IsChildAction { get; set; }
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

                // TODO: NOT DRY AT ALL
                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off) 
                {
                    context.Proceed();
                    return;
                }

                var timer = context.TimerStrategy();
                var timerResult = timer.Time(context.Proceed);

                var arguments = new Arguments(context.Arguments);

                context.MessageBroker.Publish(new TimerResultMessage(timerResult, arguments.ActionDescriptor.ActionName + "()", "MVC"));
                context.MessageBroker.Publish(new Message(arguments, context.ReturnValue as ActionResult));
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

            public class Message
            {
                public Message(Arguments arguments, ActionResult returnValue)
                {
                    var controllerDescriptor = arguments.ActionDescriptor.ControllerDescriptor;

                    ControllerName = controllerDescriptor.ControllerName;
                    ControllerType = controllerDescriptor.ControllerType;
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ActionName = arguments.ActionDescriptor.ActionName;
                    ActionResultType = returnValue.GetType();
                }

                public string ControllerName { get; set; }
                
                public Type ControllerType { get; set; }
                
                public bool IsChildAction { get; set; }
                
                public string ActionName { get; set; }
                
                public Type ActionResultType { get; set; }
            }
        }
    }
}