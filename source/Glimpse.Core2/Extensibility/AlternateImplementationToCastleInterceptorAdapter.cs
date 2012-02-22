using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core2.Extensibility
{
    public class AlternateImplementationToCastleInterceptorAdapter<T> : IInterceptor where T : class
    {
        public IAlternateImplementation<T> Implementation { get; set; }
        public ILogger Logger { get; set; }

        public AlternateImplementationToCastleInterceptorAdapter(IAlternateImplementation<T> implementation, ILogger logger)
        {
            Contract.Requires<ArgumentNullException>(implementation != null, "implementation");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");

            Implementation = implementation;
            Logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var context = new CastleInvocationToAlternateImplementationContextAdapter(invocation, Logger);
            Implementation.NewImplementation(context);
        }

        public MethodInfo MethodToImplement
        {
            get { return Implementation.MethodToImplement; }
        }
    }
}