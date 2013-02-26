using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateType
{
    public class ModelBinder : AlternateType<IModelBinder>
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

        public override bool TryCreate(IModelBinder originalObj, out IModelBinder newObj, IEnumerable<object> mixins, object[] constructorArgs)
        {
            if (originalObj == null)
            {
                newObj = null;
                return false;
            }

            var originalType = originalObj.GetType();

            if (originalType == typeof(DefaultModelBinder) && ProxyFactory.IsExtendClassEligible(originalType))
            {
                newObj = ProxyFactory.ExtendClass<DefaultModelBinder>(AllMethods);
                return true;
            }

            if (originalObj is DefaultModelBinder && ProxyFactory.IsWrapClassEligible(originalType))
            {
                newObj = ProxyFactory.WrapClass((DefaultModelBinder)originalObj, AllMethods);
                return true;
            }

            if (ProxyFactory.IsWrapInterfaceEligible<IModelBinder>(originalType))
            {
                newObj = ProxyFactory.WrapInterface(originalObj, AllMethods);
                return true;
            }

            newObj = null;
            return false;
        }

        public class BindProperty : AlternateMethod
        {
            public BindProperty() : base(typeof(DefaultModelBinder), "BindProperty", BindingFlags.Instance | BindingFlags.NonPublic)
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(context.Arguments);
                var messages = new Message(args.PropertyDescriptor, context.TargetType)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(messages);
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

            public class Message : MessageBase, ISourceMessage
            {
                public Message(PropertyDescriptor propertyDescriptor, Type modelBinderType) 
                {
                    Name = propertyDescriptor.Name;
                    Type = propertyDescriptor.PropertyType;
                    ModelBinderType = modelBinderType;
                }

                public Type ModelBinderType { get; private set; }

                public Type Type { get; private set; }

                public string Name { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }

        public class BindModel : AlternateMethod
        {
            public BindModel() : base(typeof(DefaultModelBinder), "BindModel")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(context.Arguments);
                var message = new Message(args.ModelBindingContext, context.ReturnValue, context.TargetType)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);
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

            public class Message : MessageBase, ISourceMessage
            {
                public Message(ModelBindingContext bindingContext, object rawValue, Type modelBinderType) 
                {
                    ModelName = bindingContext.ModelName;
                    ModelType = bindingContext.ModelType;
                    ModelBinderType = modelBinderType;
                    RawValue = rawValue;
                }

                public object RawValue { get; private set; }

                public Type ModelBinderType { get; private set; }

                public Type ModelType { get; private set; }

                public string ModelName { get; private set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }
    }
}