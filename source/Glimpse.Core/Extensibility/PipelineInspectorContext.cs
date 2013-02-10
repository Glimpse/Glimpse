using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The context passed into the <c>Setup</c> method of <see cref="IPipelineInspector"/>.
    /// </summary>
    public class PipelineInspectorContext : IPipelineInspectorContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInspectorContext" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="proxyFactory">The proxy factory.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="timerStrategy">The timer strategy.</param>
        /// <param name="runtimePolicyStrategy">The runtime policy strategy.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameter if <c>null</c>.</exception>
        public PipelineInspectorContext(ILogger logger, IProxyFactory proxyFactory, IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
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
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
            RuntimePolicyStrategy = runtimePolicyStrategy;
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the proxy factory.
        /// </summary>
        /// <value>
        /// The proxy factory.
        /// </value>
        public IProxyFactory ProxyFactory { get; set; }

        /// <summary>
        /// Gets or sets the timer strategy.
        /// </summary>
        /// <value>
        /// The timer strategy.
        /// </value>
        public Func<IExecutionTimer> TimerStrategy { get; set; }

        /// <summary>
        /// Gets or sets the message broker.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the runtime policy strategy.
        /// </summary>
        /// <value>
        /// The runtime policy strategy.
        /// </value>
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
    }
}