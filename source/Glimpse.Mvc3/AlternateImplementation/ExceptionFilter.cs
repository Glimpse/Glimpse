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
    public class ExceptionFilter : Alternate<IExceptionFilter>
    {
        public ExceptionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation> AllMethods()
        {
            yield return new OnException();
        }

        public class OnException : IAlternateImplementation
        {
            public OnException()
            {
                MethodToImplement = typeof(IExceptionFilter).GetMethod("OnException");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message((ExceptionContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : FilterMessage, IExceptionBasedFilterMessage
            {
                public Message(ExceptionContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, FilterCategory.Exception, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method) 
                {
                    ExceptionType = context.Exception.GetTypeOrNull();
                    ExceptionHandled = context.ExceptionHandled;
                }

                public Type ExceptionType { get; set; }

                public bool ExceptionHandled { get; set; }

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