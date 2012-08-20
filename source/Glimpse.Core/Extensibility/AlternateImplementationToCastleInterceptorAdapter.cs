using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateImplementationToCastleInterceptorAdapter<T> : IInterceptor where T : class
    {
        public IAlternateImplementation<T> Implementation { get; set; }
        public ILogger Logger { get; set; }

        public AlternateImplementationToCastleInterceptorAdapter(IAlternateImplementation<T> implementation, ILogger logger)
        {
            if (implementation == null) throw new ArgumentNullException("implementation");
            if (logger == null) throw new ArgumentNullException("logger");

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