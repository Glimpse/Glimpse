using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WebApi.Core.Message;

namespace Glimpse.WebApi.Core.AlternateType
{
    public class AuthorizationFilterAttribute : AlternateType<System.Web.Http.Filters.AuthorizationFilterAttribute>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public AuthorizationFilterAttribute(IProxyFactory proxyFactory)
            : base(proxyFactory)
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
            public OnAuthorization()
                : base(typeof(System.Web.Http.Filters.AuthorizationFilterAttribute), "OnAuthorization")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var actionContext = (HttpActionContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(actionContext.ControllerContext.Controller.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(actionContext.ControllerContext)
                    .AsFilterMessage(FilterCategory.Authorization, actionContext.GetTypeOrNull())
                    .AsBoundedFilterMessage(FilterBounds.Executing)
                    .AsWebApiTimelineMessage(WebApiMvcTimelineCategory.Filter);

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
    }
}
