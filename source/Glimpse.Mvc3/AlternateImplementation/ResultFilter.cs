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
                    : base(FilterCategory.Result, FilterBounds.Executing, executedType, method, timerResult, context.Controller)
                { 
                    Canceled = context.Cancel;
                    ResultType = context.Result == null ? null : context.Result.GetType();
                    ActionName = context.Controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
                    ControllerName = context.Controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault(); 
                }

                public bool Canceled { get; set; }

                public override void BuildEvent(ITimelineEvent timelineEvent)
                {
                    base.BuildEvent(timelineEvent);

                    timelineEvent.Details.Add("Canceled", Canceled);
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
                    : base(FilterCategory.Result, FilterBounds.Executed, executedType, method, timerResult, context.Controller)
                {
                    Canceled = context.Canceled;
                    ExceptionType = context.Exception == null ? null : context.Exception.GetType();
                    ExceptionHandled = context.ExceptionHandled;
                    ResultType = context.Result == null ? null : context.Result.GetType();
                    ActionName = context.Controller.ValueProvider.GetValue("action").RawValue.ToStringOrDefault();
                    ControllerName = context.Controller.ValueProvider.GetValue("controller").RawValue.ToStringOrDefault(); 
                }

                public bool ExceptionHandled { get; set; }

                public Type ExceptionType { get; set; }

                public bool Canceled { get; set; }

                public override void BuildEvent(ITimelineEvent timelineEvent)
                {
                    base.BuildEvent(timelineEvent);

                    timelineEvent.Details.Add("Canceled", Canceled);
                    timelineEvent.Details.Add("ExceptionHandled", ExceptionHandled);
                    timelineEvent.Details.Add("ExceptionType", ExceptionType); 
                }
            }
        }
    }
}