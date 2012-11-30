using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ControllerFactory : Alternate<IControllerFactory>
    {
        public ControllerFactory(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new CreateController(new ActionInvoker(ProxyFactory), new AsyncActionInvoker(ProxyFactory));
        }

        public class CreateController : AlternateMethod
        {
            public CreateController(Alternate<ControllerActionInvoker> alternateControllerActionInvoker, Alternate<AsyncControllerActionInvoker> alternateAsyncControllerActionInvoker) : base(typeof(IControllerFactory), "CreateController")
            {
                AlternateControllerActionInvoker = alternateControllerActionInvoker;
                AlternateAsyncControllerActionInvoker = alternateAsyncControllerActionInvoker;
            }

            public Alternate<ControllerActionInvoker> AlternateControllerActionInvoker { get; set; }

            public Alternate<AsyncControllerActionInvoker> AlternateAsyncControllerActionInvoker { get; set; }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                var controller = context.ReturnValue as Controller;

                var message = new Message(new Arguments(context.Arguments), controller);

                context.MessageBroker.Publish(message);

                ProxyActionInvoker(controller);
            }

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "iController name is allowed in this usage.")]
            protected void ProxyActionInvoker(IController iController)
            {
                if (iController == null)
                {
                    return;
                }

                var asyncController = iController as AsyncController;
                if (asyncController != null)
                {
                    var originalActionInvoker = asyncController.ActionInvoker;
                    AsyncControllerActionInvoker newActionInvoker;

                    if (AlternateAsyncControllerActionInvoker.TryCreate(originalActionInvoker as AsyncControllerActionInvoker, out newActionInvoker, new[] { new ActionInvokerStateMixin() }))
                    {
                        asyncController.ActionInvoker = newActionInvoker;
                    }

                    return;
                }

                var controller = iController as Controller;
                if (controller != null)
                {
                    var originalActionInvoker = controller.ActionInvoker;
                    ControllerActionInvoker newActionInvoker;

                    if (AlternateControllerActionInvoker.TryCreate(originalActionInvoker as ControllerActionInvoker, out newActionInvoker))
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

            public class Message : MessageBase
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