using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Message;

namespace Glimpse.Mvc3.AlternateImplementation
{
    public abstract class ActionInvoker
    {
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
        public Func<IExecutionTimer> TimerStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }

        protected ActionInvoker(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker)
        {
            if (runtimePolicyStrategy == null) throw new ArgumentNullException("runtimePolicyStrategy");
            if (timerStrategy == null) throw new ArgumentNullException("timerStrategy");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");

            RuntimePolicyStrategy = runtimePolicyStrategy;
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
        }

        public static IEnumerable<IAlternateImplementation<ControllerActionInvoker>> AllMethods(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker)
        {
            yield return new InvokeActionResult<ControllerActionInvoker>(runtimePolicyStrategy, timerStrategy, messageBroker);
            yield return new InvokeActionMethod(runtimePolicyStrategy, timerStrategy, messageBroker);
        }

        public class InvokeActionResult<T> : ActionInvoker, IAlternateImplementation<T> where T : class
        {
            public InvokeActionResult(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker) : base(runtimePolicyStrategy, timerStrategy, messageBroker)
            {
            }

            public MethodInfo MethodToImplement
            {
                get { return typeof(T).GetMethod("InvokeActionResult", BindingFlags.Instance | BindingFlags.NonPublic); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                //void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult)
                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var timer = TimerStrategy();
                var timerResult = timer.Time(context.Proceed);

                MessageBroker.Publish(new TimerResultMessage(timerResult, "ActionResult Executed", "MVC"));//TODO clean this up, use a constant?
                MessageBroker.Publish(new Message(new Arguments(context.Arguments)));
            }

            public class Arguments
            {
                public Arguments(object[] args)
                {
                    ControllerContext = (ControllerContext) args[0];
                    ActionResult = (ActionResult) args[1];
                }

                public ControllerContext ControllerContext { get; set; }
                public ActionResult ActionResult { get; set; }
            }

            public class Message
            {
                public Message(Arguments arguments)
                {
                    ActionResultType = arguments.ActionResult.GetType();
                    ConrollerType = arguments.ControllerContext.Controller.GetType();
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                }

                public Type ActionResultType { get; set; }
                public Type ConrollerType { get; set; }
                public bool IsChildAction { get; set; }
            }
        }

        public class InvokeActionMethod: ActionInvoker, IAlternateImplementation<ControllerActionInvoker>
        {
            public InvokeActionMethod(Func<RuntimePolicy> runtimePolicyStrategy, Func<IExecutionTimer> timerStrategy, IMessageBroker messageBroker) : base(runtimePolicyStrategy, timerStrategy, messageBroker)
            {
            }

            public MethodInfo MethodToImplement
            {
                get { return typeof(ControllerActionInvoker).GetMethod("InvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic); }
            }
            public void NewImplementation(IAlternateImplementationContext context)
            {
                //ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
                if (RuntimePolicyStrategy() == RuntimePolicy.Off) //TODO: NOT DRY AT ALL
                {
                    context.Proceed();
                    return;
                }

                var timer = TimerStrategy();
                var timerResult = timer.Time(context.Proceed);

                var arguments = new Arguments(context.Arguments);

                MessageBroker.Publish(new TimerResultMessage(timerResult, arguments.ActionDescriptor.ActionName+"()", "MVC"));
                MessageBroker.Publish(new Message(arguments, context.ReturnValue as ActionResult));
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

            public class Message
            {
                public Message(Arguments arguments, ActionResult returnValue)
                {
                    var controllerDescriptor = arguments.ActionDescriptor.ControllerDescriptor;

                    ControllerName = controllerDescriptor.ControllerName;
                    ControllerType = controllerDescriptor.ControllerType;
                    IsChildAction = arguments.ControllerContext.IsChildAction;
                    ActionName = arguments.ActionDescriptor.ActionName;
                    ActionResultType = returnValue.GetType();
                }

                public string ControllerName { get; set; }
                public Type ControllerType { get; set; }
                public bool IsChildAction { get; set; }
                public string ActionName { get; set; }
                public Type ActionResultType { get; set; }
            }
        }
    }
}