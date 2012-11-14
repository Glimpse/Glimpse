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

                context.MessageBroker.Publish(new Message((ResultExecutingContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : BoundedFilterMessage, ICanceledBasedFilterMessage
            {
                public Message(ResultExecutingContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.Controller), GetActionName(context.Controller), FilterBounds.Executing, FilterCategory.Result, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                { 
                    Canceled = context.Cancel; 
                }

                public bool Canceled { get; set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("Canceled", Canceled);
                }
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

                context.MessageBroker.Publish(new Message((ResultExecutedContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timer));
            }

            public class Message : BoundedFilterMessage, IExceptionBasedFilterMessage, ICanceledBasedFilterMessage
            {
                public Message(ResultExecutedContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.Controller), GetActionName(context.Controller), FilterBounds.Executed, FilterCategory.Result, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                {
                    Canceled = context.Canceled;
                    ExceptionType = context.Exception.GetTypeOrNull();
                    ExceptionHandled = context.ExceptionHandled;
                }

                public bool ExceptionHandled { get; set; }

                public Type ExceptionType { get; set; }

                public bool Canceled { get; set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("Canceled", Canceled);
                    details.Add("ExceptionHandled", ExceptionHandled);
                    details.Add("ExceptionType", ExceptionType); 
                }
            }
        }
    }
}