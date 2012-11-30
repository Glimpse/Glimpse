using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ActionFilter : AlternateType<IActionFilter>
    {
        public ActionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new OnActionExecuting();
            yield return new OnActionExecuted();
        }

        public class OnActionExecuting : AlternateMethod
        {
            public OnActionExecuting() : base(typeof(IActionFilter), "OnActionExecuting")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message((ActionExecutingContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
            }

            public class Message : BoundedFilterMessage
            {
                public Message(ActionExecutingContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.ActionDescriptor), GetActionName(context.ActionDescriptor), FilterBounds.Executing, FilterCategory.Action, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                {
                }  
            }
        }

        public class OnActionExecuted : AlternateMethod
        {
            public OnActionExecuted() : base(typeof(IActionFilter), "OnActionExecuted")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message((ActionExecutedContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
            }

            public class Message : BoundedFilterMessage, IExceptionBasedFilterMessage, ICanceledBasedFilterMessage
            {
                public Message(ActionExecutedContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.ActionDescriptor), GetActionName(context.ActionDescriptor), FilterBounds.Executed, FilterCategory.Action, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                { 
                    Canceled = context.Canceled;
                    ExceptionHandled = context.ExceptionHandled;
                    ExceptionType = context.Exception.GetTypeOrNull();
                } 

                public bool Canceled { get; private set; }

                public bool ExceptionHandled { get; private set; }

                public Type ExceptionType { get; private set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("Canceled", Canceled);
                    details.Add("ExceptionHandled", ExceptionHandled);
                    details.Add("ExceptionType", ExceptionType);
                }
            }
        }
    }
}