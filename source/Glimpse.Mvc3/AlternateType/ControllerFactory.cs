using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateType
{
    public class ControllerFactory : AlternateType<IControllerFactory>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ControllerFactory(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new CreateController(new ActionInvoker(ProxyFactory), new AsyncActionInvoker(ProxyFactory))
                    });
            }
        }

        public class CreateController : AlternateMethod
        {
            public CreateController(AlternateType<IActionInvoker> alternateControllerActionInvoker, AlternateType<IAsyncActionInvoker> alternateAsyncControllerActionInvoker) : base(typeof(IControllerFactory), "CreateController")
            {
                AlternateControllerActionInvoker = alternateControllerActionInvoker;
                AlternateAsyncControllerActionInvoker = alternateAsyncControllerActionInvoker;
            }

            public AlternateType<IActionInvoker> AlternateControllerActionInvoker { get; set; }

            public AlternateType<IAsyncActionInvoker> AlternateAsyncControllerActionInvoker { get; set; }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var controller = context.ReturnValue as Controller;

                var args = new Arguments(context.Arguments); 
                var message = new Message(args.ControllerName, controller)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);

                ProxyActionInvoker(controller);
            }

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "iController name is allowed in this usage.")]
            private void ProxyActionInvoker(IController iController)
            {
                if (iController == null)
                {
                    return;
                }

                var asyncController = iController as AsyncController;
                if (asyncController != null && asyncController.ActionInvoker is IAsyncActionInvoker)
                {
                    var originalActionInvoker = asyncController.ActionInvoker as IAsyncActionInvoker;
                    IAsyncActionInvoker newActionInvoker;

                    if (AlternateAsyncControllerActionInvoker.TryCreate(originalActionInvoker, out newActionInvoker))
                    {
                        asyncController.ActionInvoker = newActionInvoker;
                    }

                    return;
                }

                var controller = iController as Controller;
                if (controller != null)
                {
                    var originalActionInvoker = controller.ActionInvoker;
                    IActionInvoker newActionInvoker;

                    if (AlternateControllerActionInvoker.TryCreate(originalActionInvoker, out newActionInvoker))
                    {
                        controller.ActionInvoker = newActionInvoker;
                    }
                }
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    RequestContext = (RequestContext)arguments[0];
                    ControllerName = (string)arguments[1];
                }

                public RequestContext RequestContext { get; set; }
                
                public string ControllerName { get; set; }
            }

            public class Message : MessageBase, ISourceMessage
            {
                public Message(string controllerName, IController controller)
                {
                    ControllerName = controllerName;
                    IsControllerResolved = false;

                    if (controller != null)
                    {
                        ControllerType = controller.GetType();
                        IsControllerResolved = true;
                    }
                }

                public bool IsControllerResolved { get; private set; }

                public Type ControllerType { get; private set; }

                public string ControllerName { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }
    }
}