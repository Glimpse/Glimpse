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

                context.MessageBroker.Publish(new Message((ActionExecutingContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : BoundedFilterMessage
            {
                public Message(ActionExecutingContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(FilterCategory.Action, FilterBounds.Executing, executedType, method, timerResult, context.Controller)
                {
                    ResultType = context.Result != null ? context.Result.GetType() : null;
                    ControllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
                    ActionName = context.ActionDescriptor.ActionName; 
                }  
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

                context.MessageBroker.Publish(new Message((ActionExecutedContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : BoundedFilterMessage, IExceptionBasedFilterMessage, ICanceledBasedFilterMessage
            {
                public Message(ActionExecutedContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(FilterCategory.Action, FilterBounds.Executed, executedType, method, timerResult, context.Controller)
                { 
                    Canceled = context.Canceled;
                    ExceptionHandled = context.ExceptionHandled;
                    ExceptionType = context.Exception != null ? context.Exception.GetType() : null;
                    ResultType = context.Result != null ? context.Result.GetType() : null;
                    ControllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
                    ActionName = context.ActionDescriptor.ActionName; 
                } 

                public bool Canceled { get; private set; }

                public bool ExceptionHandled { get; private set; }

                public Type ExceptionType { get; private set; } 

                public override void BuildEvent(ITimelineEvent timelineEvent)
                {
                    base.BuildEvent(timelineEvent);

                    timelineEvent.Details.Add("Canceled", Canceled);
                    timelineEvent.Details.Add("ExceptionHandled", ExceptionHandled);
                    timelineEvent.Details.Add("ExceptionType", ExceptionType);
                }
            }
        }
    }
}