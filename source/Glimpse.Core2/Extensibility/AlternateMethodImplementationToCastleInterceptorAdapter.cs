using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core2.Extensibility
{
    public class AlternateMethodImplementationToCastleInterceptorAdapter<T> : IInterceptor where T : class
    {
        public IAlternateMethodImplementation<T> Implementation { get; set; }
        public ILogger Logger { get; set; }

        public AlternateMethodImplementationToCastleInterceptorAdapter(IAlternateMethodImplementation<T> implementation, ILogger logger)
        {
            Contract.Requires<ArgumentNullException>(implementation != null, "implementation");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");

            Implementation = implementation;
            Logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var context = new CastleInvocationToAlternateMethodImplementationContextAdapter(invocation, Logger);
            Implementation.NewImplementation(context);
        }

        public MethodInfo MethodToImplement
        {
            get { return Implementation.MethodToImplement; }
        }
    }
}