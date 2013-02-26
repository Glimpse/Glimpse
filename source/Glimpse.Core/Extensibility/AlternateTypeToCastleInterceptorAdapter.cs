using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An adapter between Glimpse's <see cref="IAlternateType{T}"/> and Castle DynamicProxy's <see cref="IInterceptor"/>.
    /// </summary>
    public class AlternateTypeToCastleInterceptorAdapter : IInterceptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateTypeToCastleInterceptorAdapter" /> class.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="proxyFactory">The proxy factory.</param>
        /// <param name="timerStrategy">The timer strategy.</param>
        /// <param name="runtimePolicyStrategy">The runtime policy strategy.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if either <paramref name="implementation"/> or <paramref name="logger"/> are <c>null</c>.</exception>
        public AlternateTypeToCastleInterceptorAdapter(IAlternateMethod implementation, ILogger logger, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
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

        /// <summary>
        /// Gets or sets the implementation.
        /// </summary>
        /// <value>
        /// The implementation.
        /// </value>
        public IAlternateMethod Implementation { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the message broker.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the proxy factory.
        /// </summary>
        /// <value>
        /// The proxy factory.
        /// </value>
        public IProxyFactory ProxyFactory { get; set; }

        /// <summary>
        /// Gets or sets the runtime policy strategy.
        /// </summary>
        /// <value>
        /// The runtime policy strategy.
        /// </value>
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

        /// <summary>
        /// Gets or sets the timer strategy.
        /// </summary>
        /// <value>
        /// The timer strategy.
        /// </value>
        public Func<IExecutionTimer> TimerStrategy { get; set; }

        /// <summary>
        /// Gets the method to implement.
        /// </summary>
        /// <value>
        /// The method to implement.
        /// </value>
        public MethodInfo MethodToImplement
        {
            get { return Implementation.MethodToImplement; }
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var context = new CastleInvocationToAlternateMethodContextAdapter(invocation, Logger, MessageBroker, ProxyFactory, TimerStrategy, RuntimePolicyStrategy);
            Implementation.NewImplementation(context);
        }
    }
}