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
    public class ExceptionFilter : AlternateType<IExceptionFilter>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ExceptionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new OnException()
                    });
            }
        }

        public class OnException : AlternateMethod
        {
            public OnException() : base(typeof(IExceptionFilter), "OnException")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var exceptionContext = (ExceptionContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(exceptionContext.Controller)
                    .AsChildActionMessage(exceptionContext.Controller)
                    .AsFilterMessage(FilterCategory.Exception, exceptionContext.GetTypeOrNull())
                    .AsExceptionFilterMessage(exceptionContext.Exception.GetTypeOrNull(), exceptionContext.ExceptionHandled)
                    .AsMvcTimelineMessage(MvcTimelineCategory.Filter);
                 
                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, IExceptionFilterMessage, IExecutionMessage
            {
                public string ControllerName { get; set; }
                
                public string ActionName { get; set; }
                
                public FilterCategory Category { get; set; }
                
                public Type ResultType { get; set; }
                
                public Type ExceptionType { get; set; }
                
                public bool ExceptionHandled { get; set; }
                
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