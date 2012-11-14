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

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations) where T : class
        {
            return CreateProxy(instance, methodImplementations, null);
        }

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations, object mixin) where T : class
        {
            var interceptorArray = (from implementaion in methodImplementations select new AlternateImplementationToCastleInterceptorAdapter<T>(implementaion, Logger, MessageBroker, this, TimerStrategy, RuntimePolicyStrategy)).ToArray();
            var generationHook = new AlternateImplementationGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateImplementationSelector<T>();
            var options = new ProxyGenerationOptions(generationHook) { Selector = selector };
            if (mixin != null)
            {
                options.AddMixinInstance(mixin);
            }

            if (typeof(T).IsInterface)
            {
                var interfaceProxy = ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);
                Logger.Debug("Proxied interface of type '{0}' with '{1}'.", typeof(T), interfaceProxy.GetType());
                return interfaceProxy;
            }

            var classProxy = ProxyGenerator.CreateClassProxyWithTarget(instance, options, interceptorArray);
            Logger.Debug("Proxied class of type '{0}' with '{1}'.", typeof(T), classProxy.GetType());
            return classProxy;
        }

        public bool IsProxyable(object obj)
        {
            var objType = obj.GetType();

            if (objType.IsSealed)
            {
                Logger.Debug("Object of type '{0}' is not proxyable because it is sealed.", objType);
                return false;
            }

            if (obj is IProxyTargetAccessor)
            {
                Logger.Debug("Object of type '{0}' is not proxyable because it is already a proxy object.", objType);
                return false;
            }

            return true;
        }
    }
}