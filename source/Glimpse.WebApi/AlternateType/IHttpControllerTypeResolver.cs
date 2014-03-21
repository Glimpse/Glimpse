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
using System.Web.Http.Dispatcher;

namespace Glimpse.WebApi.AlternateType
{
    public class IHttpControllerTypeResolver : AlternateType<System.Web.Http.Dispatcher.IHttpControllerTypeResolver>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public IHttpControllerTypeResolver(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new GetControllerTypes()
                });
            }
        }

        public class GetControllerTypes : AlternateMethod
        {
            public GetControllerTypes()
                : base(typeof(System.Web.Http.Dispatcher.IHttpControllerTypeResolver), "GetControllerTypes")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var assembliesResolver = (IAssembliesResolver)context.Arguments[0];
                var controllerDescriptor = (HttpControllerDescriptor)context.Arguments[1];
                //var methodInfo = controllerDescriptor.ControllerType.GetMethods().First(m => m.Name.Equals(activatorContext.Method.Method, StringComparison.InvariantCultureIgnoreCase));
                var message = new Message()
                    .AsTimedMessage(timerResult)
                        .AsSourceMessage(controllerDescriptor.ControllerType, context.MethodInvocationTarget)
                     //   .AsActionMessage(controllerDescriptor.ControllerName, activatorContext.Method.Method)
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
