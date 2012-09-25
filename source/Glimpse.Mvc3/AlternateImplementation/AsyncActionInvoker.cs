using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public abstract class AsyncActionInvoker:ActionInvoker
    {
        protected AsyncActionInvoker(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker) : base(runtimePolicyStrategy, timerStrategy, messageBroker)
        {
        }

        public static new IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>> AllMethods(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker)
        {
            yield return new BeginInvokeActionMethod(runtimePolicyStrategy, timerStrategy, messageBroker);
            yield return new EndInvokeActionMethod(runtimePolicyStrategy, timerStrategy, messageBroker);
            yield return new InvokeActionResult<AsyncControllerActionInvoker>(runtimePolicyStrategy, timerStrategy, messageBroker);
        }

        public class BeginInvokeActionMethod : AsyncActionInvoker, IAlternateImplementation<AsyncControllerActionInvoker>
        {
            public BeginInvokeActionMethod(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker): base(runtimePolicyStrategy, timerStrategy, messageBroker)
            {
            }

            public MethodInfo MethodToImplement
            {
                get { return typeof(AsyncControllerActionInvoker).GetMethod("BeginInvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                //BeginInvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters, AsyncCallback callback, object state)
                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var state = (IActionInvokerState) context.Proxy;
                var timer = TimerStrategy();
                state.Arguments = new InvokeActionMethod.Arguments(context.Arguments);
                state.Offset = timer.Start();
                context.Proceed();
            }
        }

        public class EndInvokeActionMethod : AsyncActionInvoker, IAlternateImplementation<AsyncControllerActionInvoker>
        {
            public EndInvokeActionMethod(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker): base(runtimePolicyStrategy, timerStrategy, messageBroker)
            {
            }

            public MethodInfo MethodToImplement
            {
                get { return typeof(AsyncControllerActionInvoker).GetMethod("EndInvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                context.Proceed();
                var state = (IActionInvokerState) context.Proxy;
                var timer = TimerStrategy();
                var timerResult = timer.Stop(state.Offset);

                MessageBroker.Publish(new InvokeActionMethod.Message(state.Arguments, (ActionResult) context.ReturnValue));
                var eventName = string.Format("{0}.{1}()",
                                              state.Arguments.ActionDescriptor.ControllerDescriptor.ControllerName,
                                              state.Arguments.ActionDescriptor.ActionName);
                MessageBroker.Publish(new TimerResultMessage(timerResult, eventName, "ASP.NET MVC")); //TODO: This should be abstracted
            }
        }

        public interface IActionInvokerState
        {
            long Offset { get; set; }
            InvokeActionMethod.Arguments Arguments { get; set; }
        }

        public class ActionInvokerState:IActionInvokerState
        {
            public long Offset { get; set; }
            public InvokeActionMethod.Arguments Arguments { get; set; }
        }
    }
}