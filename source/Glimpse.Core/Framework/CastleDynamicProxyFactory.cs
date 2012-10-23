using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class CastleDynamicProxyFactory : IProxyFactory
    {
        public CastleDynamicProxyFactory(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Logger = logger;
            ProxyGenerator = new ProxyGenerator();
        }

        public ILogger Logger { get; set; }
        
        public ProxyGenerator ProxyGenerator { get; set; }

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations) where T : class
        {
            return CreateProxy(instance, methodImplementations, null);
        }

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations, object mixin) where T : class
        {
            var interceptorArray = (from implementaion in methodImplementations select new AlternateImplementationToCastleInterceptorAdapter<T>(implementaion, Logger)).ToArray();
            var generationHook = new AlternateImplementationGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateImplementationSelector<T>();
            var options = new ProxyGenerationOptions(generationHook) { Selector = selector };
            if (mixin != null)
            {
                options.AddMixinInstance(mixin);
            }

            if (typeof(T).IsInterface)
            {
                return ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);
            }

            return ProxyGenerator.CreateClassProxyWithTarget(instance, options, interceptorArray);
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