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
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    new Arguments(context.Arguments), 
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

            public class Message : ExecutionMessage, IActionBasedMessage
            {
                private static MethodInfo executedMethod = typeof(ActionResult).GetMethod("ExecuteResult");

                public Message(Arguments arguments, TimerResult timerResult)
                    : base(arguments.ActionResult.GetType(), executedMethod, timerResult)
                {
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ActionName = arguments.ControllerContext.Controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
                    ControllerName = arguments.ControllerContext.Controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault(); 
                }
                 
                public string ControllerName { get; private set; }

                public string ActionName { get; private set; }
                 
                public override void BuildEvent(ITimelineEvent timelineEvent)
                {
                    base.BuildEvent(timelineEvent);

                    timelineEvent.Title = string.Format("InvokeActionResult - {0}:{1}", ControllerName, ActionName);
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

            public class Message : ExecutionMessage, IActionBasedMessage
            {
                public Message(Arguments arguments, ActionResult returnValue, MethodInfo method, TimerResult timerResult)
                    : base(arguments.ActionDescriptor.ControllerDescriptor.ControllerType, method, timerResult) 
                {
                    var controllerDescriptor = arguments.ActionDescriptor.ControllerDescriptor;
                     
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ResultType = returnValue.GetType(); 
                    ActionName = arguments.ActionDescriptor.ActionName;
                    ControllerName = controllerDescriptor.ControllerName;
                }

                public string ControllerName { get; set; }  
                
                public string ActionName { get; set; }

                public Type ResultType { get; set; }

                public override void BuildEvent(ITimelineEvent timelineEvent)
                {
                    base.BuildEvent(timelineEvent);

                    timelineEvent.Title = string.Format("InvokeActionMethod - {0}:{1}", ControllerName, ActionName); 
                }
            }
        }
    }
}