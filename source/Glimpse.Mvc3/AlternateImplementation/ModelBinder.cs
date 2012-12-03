using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ModelBinder : AlternateType<DefaultModelBinder>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ModelBinder(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new BindModel(),
                    new BindProperty()
                }); 
            }
        }

        public class BindProperty : AlternateMethod
        {
            public BindProperty() : base(typeof(DefaultModelBinder), "BindProperty", BindingFlags.Instance | BindingFlags.NonPublic)
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.TargetType));
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
                public Message(Arguments arguments, Type modelBinderType)
                {
                    Name = arguments.PropertyDescriptor.Name;
                    Type = arguments.PropertyDescriptor.PropertyType;
                    ModelBinderType = modelBinderType;
                }

                public Type ModelBinderType { get; set; }

                public Type Type { get; set; }

                public string Name { get; set; }
            }
        }

        public class BindModel : AlternateMethod
        {
            public BindModel() : base(typeof(DefaultModelBinder), "BindModel")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.TargetType, context.ReturnValue));
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
                public Message(Arguments arguments, Type modelBinderType, object rawValue)
                {
                    var modelBindingContext = arguments.ModelBindingContext;
                    ModelName = modelBindingContext.ModelName;
                    ModelType = modelBindingContext.ModelType;
                    ModelBinderType = modelBinderType;
                    RawValue = rawValue;
                }

                public object RawValue { get; set; }

                public Type ModelBinderType { get; set; }

                public Type ModelType { get; set; }

                public string ModelName { get; set; }
            }
        }
    }
}