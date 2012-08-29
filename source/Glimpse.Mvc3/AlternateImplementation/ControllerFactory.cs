using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Glimpse.Core.Extensibility;
using Glimpse.Core;

namespace Glimpse.Mvc3.AlternateImplementation
{
    public abstract class ControllerFactory
    {
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }
        public IProxyFactory ProxyFactory { get; set; }
        public Func<IExecutionTimer> TimerStrategy { get; set; }

        public static IEnumerable<IAlternateImplementation<IControllerFactory>> AllMethods(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy)
        {
            yield return new CreateController(runtimePolicyStrategy, messageBroker, proxyFactory, timerStrategy);
        }

        protected ControllerFactory(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy)
        {
            if (runtimePolicyStrategy == null) throw new ArgumentNullException("runtimePolicyStrategy");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");
            if (proxyFactory == null) throw new ArgumentNullException("proxyFactory");
            if (timerStrategy == null) throw new ArgumentNullException("timerStrategy");

            RuntimePolicyStrategy = runtimePolicyStrategy;
            MessageBroker = messageBroker;
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
        }

        protected void ProxyActionInvoker(IController iController)
        {
            if (iController == null)
                return;

            var proxyFactory = ProxyFactory;

            var asyncController = iController as AsyncController;
            if (asyncController != null)
            {
                var actionInvoker = asyncController.ActionInvoker;
                if (proxyFactory.IsProxyable(actionInvoker))
                {
                    var proxiedAsyncInvoker = proxyFactory.CreateProxy(actionInvoker as AsyncControllerActionInvoker, AsyncActionInvoker.AllMethods(RuntimePolicyStrategy, TimerStrategy, MessageBroker), new AsyncActionInvoker.ActionInvokerState());
                    asyncController.ActionInvoker = proxiedAsyncInvoker;
                }

                return;
            }

            var controller = iController as Controller;
            if (controller != null)
            {
                var actionInvoker = controller.ActionInvoker;

                if (proxyFactory.IsProxyable(actionInvoker))
                {
                    var proxiedActionInvoker = proxyFactory.CreateProxy(actionInvoker as ControllerActionInvoker, ActionInvoker.AllMethods(RuntimePolicyStrategy, TimerStrategy, MessageBroker));

                    controller.ActionInvoker = proxiedActionInvoker;
                }

                return;
            }
        }


        public class CreateController : ControllerFactory, IAlternateImplementation<IControllerFactory>
        {
            public CreateController(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy): base(runtimePolicyStrategy, messageBroker, proxyFactory, timerStrategy)
            {
            }

            public MethodInfo MethodToImplement
            {
                get { return typeof (IControllerFactory).GetMethod("CreateController"); }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                context.Proceed();

                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                    return;

                var controller = context.ReturnValue as Controller;

                var message = new Message(new Arguments(context.Arguments), controller);

                MessageBroker.Publish(message);

                ProxyActionInvoker(controller);
            }

            public class Arguments
            {
                public Arguments(object[] arguments)
                {
                    RequestContext = (RequestContext) arguments[0];
                    ControllerName = (string) arguments[1];
                }

                public RequestContext RequestContext { get; set; }
                public string ControllerName { get; set; }
            }

            public class Message
            {
                public Message(Arguments arguments, IController controller)
                {
                    RouteData = arguments.RequestContext.RouteData;
                    ControllerName = arguments.ControllerName;
                    IsControllerResolved = false;

                    if (controller != null)
                    {
                        ControllerType = controller.GetType();
                        IsControllerResolved = true;
                    }
                }

                public bool IsControllerResolved { get; set; }
                public Type ControllerType { get; set; }
                public RouteData RouteData { get; set; }
                public string ControllerName { get; set; }
            }
        }
    }
}