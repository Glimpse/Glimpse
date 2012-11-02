using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ActionFilter : Alternate<IActionFilter>
    {
        public ActionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IActionFilter>> AllMethods()
        {
            yield return new OnActionExecuting();
            yield return new OnActionExecuted();
        }

        public class OnActionExecuting : IAlternateImplementation<IActionFilter>
        {
            public OnActionExecuting()
            {
                MethodToImplement = typeof(IActionFilter).GetMethod("OnActionExecuting");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var timer = context.ProceedWithTimerIfAllowed();

                if (timer == null)
                {
                    return;
                }

                context.MessageBroker.PublishMany(
                    new Message((ActionExecutingContext)context.Arguments[0]), 
                    new TimerResultMessage(timer, "OnActionExecuting", "ActionFilter"));
            }

            public class Message : MessageBase
            {
                public Message(ActionExecutingContext context)
                {
                    ActionName = context.ActionDescriptor.ActionName;
                    ResultType = context.Result != null ? context.Result.GetType() : null;
                }

                public string ActionName { get; private set; }

                public Type ResultType { get; set; }
            }
        }

        public class OnActionExecuted : IAlternateImplementation<IActionFilter>
        {
            public OnActionExecuted()
            {
                MethodToImplement = typeof(IActionFilter).GetMethod("OnActionExecuted");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var timer = context.ProceedWithTimerIfAllowed();

                if (timer == null)
                {
                    return;
                }

                context.MessageBroker.PublishMany(
                    new Message((ActionExecutedContext)context.Arguments[0]), 
                    new TimerResultMessage(timer, "OnActionExecuted", "ActionFilter"));
            }

            public class Message : MessageBase
            {
                public Message(ActionExecutedContext context)
                {
                    ActionName = context.ActionDescriptor.ActionName;
                    ControllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
                    IsCanceled = context.Canceled;
                    ExceptionHandled = context.ExceptionHandled;
                    ExceptionType = context.Exception != null ? context.Exception.GetType() : null;
                    ResultType = context.Result != null ? context.Result.GetType() : null;
                }

                public string ActionName { get; private set; }

                public string ControllerName { get; private set; }

                public bool IsCanceled { get; private set; }

                public bool ExceptionHandled { get; private set; }

                public Type ExceptionType { get; private set; }

                public Type ResultType { get; private set; }
            }
        }
    }
}