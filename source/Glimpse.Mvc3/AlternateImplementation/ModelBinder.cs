using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ModelBinder : Alternate<DefaultModelBinder>
    {
        public ModelBinder(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<DefaultModelBinder>> AllMethods()
        {
            yield return new BindModel();
            yield return new BindProperty();
        }

        public class BindProperty : IAlternateImplementation<DefaultModelBinder>
        {
            public BindProperty()
            {
                MethodToImplement = typeof(DefaultModelBinder).GetMethod("BindProperty", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments)));
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    ControllerContext = (ControllerContext)arguments[0];
                    ModelBindingContext = (ModelBindingContext)arguments[1];
                    PropertyDescriptor = (PropertyDescriptor)arguments[2];
                }

                public ControllerContext ControllerContext { get; set; }

                public ModelBindingContext ModelBindingContext { get; set; }

                public PropertyDescriptor PropertyDescriptor { get; set; }
            }

            public class Message : MessageBase
            {
                public Message(Arguments arguments)
                {
                    // TODO: Add meaningful properties to message w/ test
                }
            }
        }

        public class BindModel : IAlternateImplementation<DefaultModelBinder>
        {
            public BindModel()
            {
                MethodToImplement = typeof(DefaultModelBinder).GetMethod("BindModel");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments)));
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    ControllerContext = (ControllerContext)arguments[0];
                    ModelBindingContext = (ModelBindingContext)arguments[1];
                }

                public ControllerContext ControllerContext { get; set; }

                public ModelBindingContext ModelBindingContext { get; set; }
            }

            public class Message
            {
                public Message(Arguments arguments)
                {
                    // TODO: Add meaningful properties to message w/ test
                }
            }
        }
    }
}