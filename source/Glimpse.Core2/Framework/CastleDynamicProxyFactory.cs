using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class CastleDynamicProxyFactory : IProxyFactory
    {
        public ILogger Logger { get; set; }
        public ProxyGenerator ProxyGenerator { get; set; }

        public CastleDynamicProxyFactory(ILogger logger)
        {
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Ensures(Logger != null);
            Contract.Ensures(ProxyGenerator != null);

            Logger = logger;
            ProxyGenerator = new ProxyGenerator();
        }

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations) where T : class
        {
            return CreateProxy(instance, methodImplementations, null);
        }

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateImplementation<T>> methodImplementations, object mixin) where T : class
        {
            var interceptorArray = (from implementaion in methodImplementations select new AlternateImplementationToCastleInterceptorAdapter<T>(implementaion, Logger)).ToArray();
            var generationHook = new AlternateImplementationGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateImplementationSelector<T>(interceptorArray);
            var options = new ProxyGenerationOptions(generationHook) {Selector = selector};
            if (mixin != null) options.AddMixinInstance(mixin);

            return ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);
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

            //TODO: Figure out if this is needed, it appears that it does not have to be given the .CreateInterfaceProxyWithTarget() call.
            //if (objType.GetConstructor(new Type[] {}) == null)
            //{
            //    Logger.Debug("Object of type '{0}' is not proxyable because it does not have a public parameterless constructor.", objType);
            //    return false;
            //}

            return true;
        }
    }
}