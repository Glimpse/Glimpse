using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
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
                var timer = context.ProceedWithTimerIfAllowed();

                if (timer == null)
                {
                    return;
                }

                context.MessageBroker.PublishMany(
                    new Message((AuthorizationContext)context.Arguments[0]), 
                    new TimerResultMessage(timer, "OnAuthorization", "AuthorizationFilter"));
            }

            public class Message
            {
                public Message(AuthorizationContext argument)
                {
                    ActionName = argument.ActionDescriptor.ActionName;
                    ResultType = argument.Result == null ? null : argument.Result.GetType();
                }

                public Type ResultType { get; set; }

                public string ActionName { get; set; }
            }
        }
    }
}