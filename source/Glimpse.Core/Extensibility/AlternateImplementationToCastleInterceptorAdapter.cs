using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class AlternateImplementationToCastleInterceptorAdapter<T> : IInterceptor where T : class
    {
        public AlternateImplementationToCastleInterceptorAdapter(IAlternateImplementation<T> implementation, ILogger logger, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            if (implementation == null)
            {
                throw new ArgumentNullException("implementation");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Implementation = implementation;
            Logger = logger;
            MessageBroker = messageBroker;
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
            RuntimePolicyStrategy = runtimePolicyStrategy;
        }

        public IAlternateImplementation<T> Implementation { get; set; }
        
        public ILogger Logger { get; set; }

        public IMessageBroker MessageBroker { get; set; }

        public IProxyFactory ProxyFactory { get; set; }

        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

        public Func<IExecutionTimer> TimerStrategy { get; set; }

        public MethodInfo MethodToImplement
        {
            get { return Implementation.MethodToImplement; }
        }

        public void Intercept(IInvocation invocation)
        {
            var context = new CastleInvocationToAlternateImplementationContextAdapter(invocation, Logger, MessageBroker, ProxyFactory, TimerStrategy, RuntimePolicyStrategy);
            Implementation.NewImplementation(context);
        }
    }
}