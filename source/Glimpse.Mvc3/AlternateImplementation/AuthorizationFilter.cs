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

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new OnAuthorization();
        }

        public class OnAuthorization : IAlternateMethod
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

            public class Message : ActionFilterMessage
            {
                public Message(AuthorizationContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.ActionDescriptor), GetActionName(context.ActionDescriptor), FilterCategory.Authorization, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                {
                }
            }
        }
    }
}