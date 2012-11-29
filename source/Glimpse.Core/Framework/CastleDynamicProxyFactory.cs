using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class CastleDynamicProxyFactory : IProxyFactory
    {
        public CastleDynamicProxyFactory(ILogger logger, IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (messageBroker == null)
            {
                throw new ArgumentNullException("messageBroker");
            }

            if (timerStrategy == null)
            {
                throw new ArgumentNullException("timerStrategy");
            }

            if (runtimePolicyStrategy == null)
            {
                throw new ArgumentNullException("runtimePolicyStrategy");
            }

            Logger = logger;
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
            RuntimePolicyStrategy = runtimePolicyStrategy;
            ProxyGenerator = new ProxyGenerator();
        }

        public ILogger Logger { get; set; }
        
        public ProxyGenerator ProxyGenerator { get; set; }

        public IMessageBroker MessageBroker { get; set; }

        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

        public Func<IExecutionTimer> TimerStrategy { get; set; }

        public bool IsWrapInterfaceEligible(Type type)
        {
            if (!type.IsInterface)
            {
                return false;
            }

            return IsGenerallyEligable(type);
        }

        public T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return WrapInterface(instance, methodImplementations, Enumerable.Empty<object>());
        }

        public T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            CheckInput(instance, methodImplementations, mixins);

            var options = CreateProxyOptions<T>(methodImplementations, mixins);
            var wrapper = new CastleDynamicProxyWrapper<T>();
            options.AddMixinInstance(wrapper);

            var interceptorArray = CreateInterceptorArray(methodImplementations);

            var result = ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);

            wrapper.ProxyTargetAccessor = result as IProxyTargetAccessor;

            return result;
        }

        public bool IsWrapClassEligible(Type type)
        {
            return IsExtendClassEligible(type);
        }

        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return WrapClass<T>(instance, methodImplementations, Enumerable.Empty<object>());
        }

        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            return WrapClass<T>(instance, methodImplementations, mixins, null);
        }

        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class
        {
            CheckInput(instance, methodImplementations, mixins); // null constructorArguments is valid input

            var options = CreateProxyOptions<T>(methodImplementations, mixins);
            var wrapper = new CastleDynamicProxyWrapper<T>();
            options.AddMixinInstance(wrapper);

            var interceptorArray = CreateInterceptorArray(methodImplementations);

            var result = (T)ProxyGenerator.CreateClassProxyWithTarget(typeof(T), instance, options, constructorArguments.ToArray(), interceptorArray);

            wrapper.ProxyTargetAccessor = result as IProxyTargetAccessor;

            return result;
        }

        public bool IsExtendClassEligible(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return IsGenerallyEligable(type);
        }

        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return ExtendClass<T>(methodImplementations, Enumerable.Empty<object>());
        }

        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            return ExtendClass<T>(methodImplementations, mixins, null);
        }

        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class
        {
            CheckInput(methodImplementations, mixins); // null constructorArguments is valid input

            var options = CreateProxyOptions<T>(methodImplementations, mixins);
            var interceptorArray = CreateInterceptorArray(methodImplementations);

            return (T)ProxyGenerator.CreateClassProxy(typeof(T), options, constructorArguments.ToArray(), interceptorArray);
        }

        private void CheckInput(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins)
        {
            if (methodImplementations == null)
            {
                throw new ArgumentNullException("methodImplementations");
            }

            if (mixins == null)
            {
                throw new ArgumentNullException("mixins");
            }
        }

        private void CheckInput(object instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins)
        {
            CheckInput(methodImplementations, mixins);

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
        }

        private bool IsGenerallyEligable(Type type)
        {
            return !type.IsSealed && type.IsAssignableFrom(typeof(IProxyTargetAccessor));
        }

        private ProxyGenerationOptions CreateProxyOptions<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins)
        {
            var generationHook = new AlternateImplementationGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateImplementationSelector();
            var options = new ProxyGenerationOptions(generationHook) { Selector = selector };

            if (mixins != null)
            {
                foreach (var mixin in mixins)
                {
                    options.AddMixinInstance(mixin);
                }
            }

            return options;
        }

        private IInterceptor[] CreateInterceptorArray(IEnumerable<IAlternateMethod> methodImplementations)
        {
            return (from implementaion in methodImplementations select new AlternateImplementationToCastleInterceptorAdapter(implementaion, Logger, MessageBroker, this, TimerStrategy, RuntimePolicyStrategy)).ToArray();
        }
    }
}