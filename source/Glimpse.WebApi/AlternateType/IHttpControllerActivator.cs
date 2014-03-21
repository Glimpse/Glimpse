using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.WebApi.Message;
using System.Net.Http;

namespace Glimpse.WebApi.AlternateType
{
    public class IHttpControllerActivator : AlternateType<System.Web.Http.Dispatcher.IHttpControllerActivator>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public IHttpControllerActivator(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new Create()
                });
            }
        }

        public class Create : AlternateMethod
        {
            public Create()
                : base(typeof(System.Web.Http.Dispatcher.IHttpControllerActivator), "Create")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var activatorContext = (HttpRequestMessage)context.Arguments[0];
                var controllerDescriptor = (HttpControllerDescriptor)context.Arguments[1];
                var methodInfo = controllerDescriptor.ControllerType.GetMethods().First(m => m.Name.Equals(activatorContext.Method.Method, StringComparison.InvariantCultureIgnoreCase));
                var message = new Message()
                    .AsTimedMessage(timerResult)
                        .AsSourceMessage(controllerDescriptor.ControllerType, context.MethodInvocationTarget)
                        .AsActionMessage(controllerDescriptor.ControllerName, activatorContext.Method.Method)
                    //    .AsFilterMessage(FilterCategory.Authorization, actionContext.GetTypeOrNull())
                        .AsBoundedFilterMessage(FilterBounds.Executing)
                        .AsWebApiTimelineMessage(WebApiMvcTimelineCategory.Controller);

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
