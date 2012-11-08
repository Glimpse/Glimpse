using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

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
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    (ActionExecutingContext)context.Arguments[0],
                    context.InvocationTarget.GetType(),
                    context.MethodInvocationTarget,
                    timer));
            }

            public class Message : ExecutionMessage
            {
                public Message(ActionExecutingContext context, Type filterType, MethodInfo method, TimerResult timerResult) : base(FilterCategory.Action, filterType, method, timerResult, context.Controller)
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
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                        (ActionExecutedContext)context.Arguments[0], 
                        context.InvocationTarget.GetType(), 
                        context.MethodInvocationTarget, 
                        timer));
            }

            public class Message : ExecutionMessage
            {
                public Message(ActionExecutedContext context, Type filterType, MethodInfo method, TimerResult timerResult) : base(FilterCategory.Action, filterType, method, timerResult, context.Controller)
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