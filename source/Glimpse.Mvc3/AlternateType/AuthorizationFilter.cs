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
    public class AuthorizationFilter : AlternateType<IAuthorizationFilter>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public AuthorizationFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new OnAuthorization()
                }); 
            }
        }

        public class OnAuthorization : AlternateMethod
        {
            public OnAuthorization() : base(typeof(IAuthorizationFilter), "OnAuthorization")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var authorizationContext = (AuthorizationContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(authorizationContext.ActionDescriptor)
                    .AsChildActionMessage(authorizationContext.Controller)
                    .AsFilterMessage(FilterCategory.Authorization, authorizationContext.GetTypeOrNull())
                    .AsMvcTimelineMessage(Glimpse.Mvc.Message.Timeline.Filter);

                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, IFilterMessage, IExecutionMessage
            {
                public string ControllerName { get; set; }
                
                public string ActionName { get; set; }
                
                public FilterCategory Category { get; set; }
                
                public Type ResultType { get; set; }
                
                public bool IsChildAction { get; set; }
                
                public Type ExecutedType { get; set; }
                
                public MethodInfo ExecutedMethod { get; set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public string EventName { get; set; }
                
                public TimelineCategory EventCategory { get; set; }
                
                public string EventSubText { get; set; }
            }
        }
    }
}