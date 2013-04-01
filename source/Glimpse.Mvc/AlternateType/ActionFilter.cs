using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateType
{
    public class ActionFilter : AlternateType<IActionFilter>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ActionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new OnActionExecuting(),
                    new OnActionExecuted()
                }); 
            }
        }

        public class OnActionExecuting : AlternateMethod
        {
            public OnActionExecuting() : base(typeof(IActionFilter), "OnActionExecuting")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var actionContext = (ActionExecutingContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(actionContext.Controller)
                    .AsChildActionMessage(actionContext.Controller)
                    .AsFilterMessage(FilterCategory.Action, actionContext.GetTypeOrNull())
                    .AsBoundedFilterMessage(FilterBounds.Executing)
                    .AsMvcTimelineMessage(MvcTimelineCategory.Filter);
                
                context.MessageBroker.Publish(message); 
            }

            public class Message : MessageBase, IBoundedFilterMessage, IExecutionMessage
            {
                public string ControllerName { get; set; }

                public string ActionName { get; set; }

                public FilterCategory Category { get; set; }

                public Type ResultType { get; set; }
                
                public FilterBounds Bounds { get; set; }

                public bool IsChildAction { get; set; }
                
                public Type ExecutedType { get; set; }
                
                public MethodInfo ExecutedMethod { get; set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public string EventName { get; set; }
                
                public TimelineCategoryItem EventCategory { get; set; }
                
                public string EventSubText { get; set; }
            } 
        }

        public class OnActionExecuted : AlternateMethod
        {
            public OnActionExecuted() : base(typeof(IActionFilter), "OnActionExecuted")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var resultContext = (ActionExecutedContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(resultContext.Controller)
                    .AsChildActionMessage(resultContext.Controller)
                    .AsFilterMessage(FilterCategory.Action, resultContext.GetTypeOrNull())
                    .AsBoundedFilterMessage(FilterBounds.Executed)
                    .AsCanceledFilterMessage(resultContext.Canceled)
                    .AsExceptionFilterMessage(resultContext.Exception.GetTypeOrNull(), resultContext.ExceptionHandled)
                    .AsMvcTimelineMessage(MvcTimelineCategory.Filter);

                context.MessageBroker.Publish(message);  
            }

            public class Message : MessageBase, IExceptionFilterMessage, IBoundedFilterMessage, ICanceledBasedFilterMessage, IExecutionMessage
            {
                public string ControllerName { get; set; }

                public string ActionName { get; set; }

                public FilterCategory Category { get; set; }

                public Type ResultType { get; set; }
                
                public Type ExceptionType { get; set; }
                
                public bool ExceptionHandled { get; set; }
                
                public FilterBounds Bounds { get; set; }
                
                public bool Canceled { get; set; }
                
                public bool IsChildAction { get; set; }
                
                public Type ExecutedType { get; set; }
                
                public MethodInfo ExecutedMethod { get; set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public string EventName { get; set; }
                
                public TimelineCategoryItem EventCategory { get; set; }
                
                public string EventSubText { get; set; }
            } 
        }
    }
}