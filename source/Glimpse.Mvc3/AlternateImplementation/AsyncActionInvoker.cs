using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Message;

namespace Glimpse.Mvc3.AlternateImplementation
{
    public abstract class AsyncActionInvoker
    {
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
        public Func<IExecutionTimer> TimerStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }

        protected AsyncActionInvoker(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker)
        {
            if (runtimePolicyStrategy == null) throw new ArgumentNullException("runtimePolicyStrategy");
            if (timerStrategy == null) throw new ArgumentNullException("timerStrategy");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");

            RuntimePolicyStrategy = runtimePolicyStrategy;
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
        }

        public static IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>> AllMethods(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker)
        {
            yield return new BeginInvokeActionMethod(runtimePolicyStrategy, timerStrategy, messageBroker);
            yield return new EndInvokeActionMethod(runtimePolicyStrategy, timerStrategy, messageBroker);
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
                state.Arguments = new Arguments(context.Arguments);
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

                MessageBroker.Publish(new ActionInvoker.InvokeActionMethod.Message(state.Arguments, (ActionResult) context.ReturnValue));
                var eventName = string.Format("{0}.{1}()",
                                              state.Arguments.ActionDescriptor.ControllerDescriptor.ControllerName,
                                              state.Arguments.ActionDescriptor.ActionName);
                MessageBroker.Publish(new TimerResultMessage(timerResult, eventName, "ASP.NET MVC")); //TODO: This should be abstracted
            }
        }

        public class Arguments
        {
            public Arguments(object[] arguments)
            {
                ControllerContext = (ControllerContext)arguments[0];
                ActionDescriptor = (ActionDescriptor)arguments[1];
                Parameters = (IDictionary<string, object>)arguments[2];

                if (arguments.Length == 5)
                {
                    IsAsync = true;
                    Callback = (AsyncCallback)arguments[3];
                    State = arguments[4];
                }
            }

            public ControllerContext ControllerContext { get; set; }
            public ActionDescriptor ActionDescriptor { get; set; }
            public IDictionary<string, object> Parameters { get; set; }
            public AsyncCallback Callback { get; set; }
            public object State { get; set; }
            public bool IsAsync { get; set; }
        }

        public interface IActionInvokerState
        {
            long Offset { get; set; }
            Arguments Arguments { get; set; }
        }

        public class ActionInvokerState:IActionInvokerState
        {
            public long Offset { get; set; }
            public Arguments Arguments { get; set; }
        }
    }
}