using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ExceptionFilter : Alternate<IExceptionFilter>
    {
        public ExceptionFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IExceptionFilter>> AllMethods()
        {
            yield return new OnException();
        }

        public class OnException : IAlternateImplementation<IExceptionFilter>
        {
            public OnException()
            {
                MethodToImplement = typeof(IExceptionFilter).GetMethod("OnException");
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
                    new Message((ExceptionContext)context.Arguments[0]), 
                    new TimerResultMessage(timer, "ExceptionContext", "ExceptionFilter"));
            }

            public class Message
            {
                public Message(ExceptionContext exceptionContext)
                {
                    ExceptionHandled = exceptionContext.ExceptionHandled;
                    ExceptionType = exceptionContext.Exception == null ? null : exceptionContext.Exception.GetType();
                    ResultType = exceptionContext.Result == null ? null : exceptionContext.Result.GetType();
                }

                public Type ResultType { get; set; }

                public Type ExceptionType { get; set; }

                public bool ExceptionHandled { get; set; }
            }
        }
    }
}