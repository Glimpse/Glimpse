using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ControllerFactory
    {
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }

        public class CreateController : ControllerFactory, IAlternateImplementation<IControllerFactory>
        {
            public CreateController(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker)
            {
                if (runtimePolicyStrategy == null) throw new ArgumentNullException("runtimePolicyStrategy");
                if (messageBroker == null) throw new ArgumentNullException("messageBroker");

                RuntimePolicyStrategy = runtimePolicyStrategy;
                MessageBroker = messageBroker;
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

                var controller = context.ReturnValue as IController;

                var message = new Message(new Arguments(context.Arguments), controller);

                //TODO: Need to proxy controller...

                MessageBroker.Publish(message);
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