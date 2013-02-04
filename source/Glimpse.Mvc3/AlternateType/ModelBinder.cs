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
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.TargetType, context.MethodInvocationTarget));
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
                public Message(Arguments arguments, Type modelBinderType, MethodInfo executedMethod) : base(modelBinderType, executedMethod)
                {
                    Name = arguments.PropertyDescriptor.Name;
                    Type = arguments.PropertyDescriptor.PropertyType;
                    ModelBinderType = modelBinderType;
                }

                public Type ModelBinderType { get; private set; }

                public Type Type { get; private set; }

                public string Name { get; private set; }
            }
        }

        public class BindModel : AlternateMethod
        {
            public BindModel() : base(typeof(DefaultModelBinder), "BindModel")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.TargetType, context.ReturnValue, context.MethodInvocationTarget));
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

            public class Message : MessageBase
            {
                public Message(Arguments arguments, Type modelBinderType, object rawValue, MethodInfo executedMethod) : base(modelBinderType, executedMethod)
                {
                    var modelBindingContext = arguments.ModelBindingContext;
                    ModelName = modelBindingContext.ModelName;
                    ModelType = modelBindingContext.ModelType;
                    ModelBinderType = modelBinderType;
                    RawValue = rawValue;
                }

                public object RawValue { get; private set; }

                public Type ModelBinderType { get; private set; }

                public Type ModelType { get; private set; }

                public string ModelName { get; private set; }
            }
        }
    }
}