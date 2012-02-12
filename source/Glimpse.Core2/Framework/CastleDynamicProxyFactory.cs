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

        public T CreateProxy<T>(T instance, IEnumerable<IAlternateMethodImplementation<T>> methodImplementations) where T : class
        {
            var interceptorArray = (from implementaion in methodImplementations select new AlternateMethodImplementationToCastleInterceptorAdapter<T>(implementaion, Logger)).ToArray();
            var generationHook = new AlternateMethodImplementationGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateMethodImplementationSelector<T>(interceptorArray);
            var options = new ProxyGenerationOptions(generationHook) {Selector = selector};

            return ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);
        }
    }
}