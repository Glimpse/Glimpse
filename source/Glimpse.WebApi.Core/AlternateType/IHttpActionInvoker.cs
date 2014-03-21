using System.Collections.Generic;
using System.Web.Http.Controllers;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WebApi.Core.Message;
using System;
using System.Reflection;

namespace Glimpse.WebApi.Core.AlternateType
{
    public class IHttpActionInvoker : AlternateType<System.Web.Http.Controllers.IHttpActionInvoker>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public IHttpActionInvoker(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new InvokeActionAsync()
                });
            }
        }

        public class InvokeActionAsync : AlternateMethod
        {
            public InvokeActionAsync()
                : base(typeof(System.Web.Http.Controllers.IHttpActionInvoker), "InvokeActionAsync")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var actionContext = (HttpActionContext)context.Arguments[0];
                var message = new Message()
                    .AsTimedMessage(timerResult)
                    .AsSourceMessage(actionContext.ControllerContext.Controller.GetType(), context.MethodInvocationTarget)
                    .AsActionMessage(actionContext.ControllerContext)
                    .AsFilterMessage(FilterCategory.Action, actionContext.GetTypeOrNull())
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
