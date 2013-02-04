using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
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
                context.MessageBroker.Publish(new Message((AuthorizationContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
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