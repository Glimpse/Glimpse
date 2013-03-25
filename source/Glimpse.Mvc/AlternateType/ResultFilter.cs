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
    public class ResultFilter : AlternateType<IResultFilter>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ResultFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new OnResultExecuting(),
                    new OnResultExecuted()
                }); 
            }
        }

        public class OnResultExecuting : AlternateMethod
        {
            public OnResultExecuting() : base(typeof(IResultFilter), "OnResultExecuting")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var resultContext = (ResultExecutingContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(resultContext.Controller)
                    .AsChildActionMessage(resultContext.Controller)
                    .AsFilterMessage(FilterCategory.Result, resultContext.GetTypeOrNull())
                    .AsBoundedFilterMessage(FilterBounds.Executing)
                    .AsCanceledFilterMessage(resultContext.Cancel)
                    .AsMvcTimelineMessage(MvcTimelineCategory.Filter);

                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, IBoundedFilterMessage, ICanceledBasedFilterMessage, IExecutionMessage
            {
                public string ControllerName { get; set; }
                
                public string ActionName { get; set; }
                
                public FilterCategory Category { get; set; }
                
                public Type ResultType { get; set; }
                
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

        public class OnResultExecuted : AlternateMethod
        {
            public OnResultExecuted() : base(typeof(IResultFilter), "OnResultExecuted")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var resultContext = (ResultExecutedContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(resultContext.Controller)
                    .AsChildActionMessage(resultContext.Controller)
                    .AsFilterMessage(FilterCategory.Result, resultContext.GetTypeOrNull())
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