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
    public class ResultFilter : Alternate<IResultFilter>
    {
        public ResultFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IResultFilter>> AllMethods()
        {
            yield return new OnResultExecuting();
            yield return new OnResultExecuted();
        }

        public class OnResultExecuting : IAlternateImplementation<IResultFilter>
        {
            public OnResultExecuting()
            {
                MethodToImplement = typeof(IResultFilter).GetMethod("OnResultExecuting");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    (ResultExecutingContext)context.Arguments[0],
                    context.InvocationTarget.GetType(),
                    context.MethodInvocationTarget,
                    timer));
            }

            public class Message : BoundedExecutionMessage
            {
                public Message(ResultExecutingContext argument, Type filterType, MethodInfo method, TimerResult timerResult) 
                    : base(FilterCategory.Result, FilterBounds.Executing, filterType, method, timerResult, argument.Controller)
                {
                    IsCanceled = argument.Cancel;
                    ResultType = argument.Result == null ? null : argument.Result.GetType();
                }

                public Type ResultType { get; set; }

                public bool IsCanceled { get; set; }
            }
        }

        public class OnResultExecuted : IAlternateImplementation<IResultFilter>
        {
            public OnResultExecuted()
            {
                MethodToImplement = typeof(IResultFilter).GetMethod("OnResultExecuted");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(
                    (ResultExecutedContext)context.Arguments[0],
                    context.InvocationTarget.GetType(),
                    context.MethodInvocationTarget,
                    timer));
            }

            public class Message : BoundedExecutionMessage
            {
                public Message(ResultExecutedContext arguments, Type filterType, MethodInfo method, TimerResult timerResult) 
                    : base(FilterCategory.Result, FilterBounds.Executed, filterType, method, timerResult, arguments.Controller)
                {
                    Canceled = arguments.Canceled;
                    ExceptionType = arguments.Exception == null ? null : arguments.Exception.GetType();
                    ExceptionHandled = arguments.ExceptionHandled;
                    ResultType = arguments.Result == null ? null : arguments.Result.GetType();
                }

                public Type ResultType { get; set; }

                public bool ExceptionHandled { get; set; }

                public Type ExceptionType { get; set; }

                public bool Canceled { get; set; }
            }
        }
    }
}