using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ResultFilter : AlternateType<IResultFilter>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ResultFilter(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new OnResultExecuting(),
                    new OnResultExecuted()
                }); 
            }
        }

        public class OnResultExecuting : AlternateMethod
        {
            public OnResultExecuting() : base(typeof(IResultFilter), "OnResultExecuting")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message((ResultExecutingContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
            }

            public class Message : BoundedFilterMessage, ICanceledBasedFilterMessage
            {
                public Message(ResultExecutingContext context, Type executedType, MethodInfo method, TimerResult timerResult)
                    : base(timerResult, GetControllerName(context.Controller), GetActionName(context.Controller), FilterBounds.Executing, FilterCategory.Result, GetResultType(context.Result), GetIsChildAction(context.Controller), executedType, method)
                { 
                    Canceled = context.Cancel; 
                }

                public bool Canceled { get; private set; }

                public override void BuildDetails(IDictionary<string, object> details)
                {
                    base.BuildDetails(details);

                    details.Add("Canceled", Canceled);
                }
            }
        }

        public class OnResultExecuted : AlternateMethod
        {
            public OnResultExecuted() : base(typeof(IResultFilter), "OnResultExecuted")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message((ResultExecutedContext)context.Arguments[0], context.InvocationTarget.GetType(), context.MethodInvocationTarget, timerResult));
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

                public bool ExceptionHandled { get; private set; }

                public Type ExceptionType { get; private set; }

                public bool Canceled { get; private set; }

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