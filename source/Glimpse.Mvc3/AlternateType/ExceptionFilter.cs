using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
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
                context.MessageBroker.Publish(new Message((ExceptionContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
            }

            public class Message : FilterMessage, IExceptionBasedFilterMessage
            {
                public Message(ExceptionContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, FilterCategory.Exception, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method) 
                {
                    ExceptionType = context.Exception.GetTypeOrNull();
                    ExceptionHandled = context.ExceptionHandled;
                }

                public Type ExceptionType { get; private set; }

                public bool ExceptionHandled { get; private set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("ExceptionHandled", ExceptionHandled);
                    details.Add("ExceptionType", ExceptionType); 
                }
            }
        }
    }
}