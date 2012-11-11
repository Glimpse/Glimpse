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
    public class AuthorizationFilter : Alternate<IAuthorizationFilter>
    {
        public AuthorizationFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IAuthorizationFilter>> AllMethods()
        {
            yield return new OnAuthorization();
        }

        public class OnAuthorization : IAlternateImplementation<IAuthorizationFilter>
        {
            public OnAuthorization()
            {
                MethodToImplement = typeof(IAuthorizationFilter).GetMethod("OnAuthorization");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message((AuthorizationContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : FilterMessage, IActionBasedFilterMessage
            {
                public Message(AuthorizationContext context, Type filterType, MethodInfo method, TimerResult timerResult) 
                    : base(FilterCategory.Authorization, filterType, method, timerResult, context.Controller)
                {
                    ResultType = context.Result == null ? null : context.Result.GetType();
                    ControllerName = context.ActionDescriptor.ControllerDescriptor.ControllerName;
                    ActionName = context.ActionDescriptor.ActionName;
                }

                public string ControllerName { get; private set; }

                public string ActionName { get; private set; }
            }
        }
    }
}