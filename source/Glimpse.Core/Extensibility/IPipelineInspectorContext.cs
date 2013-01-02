using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of a context that is used by the <see cref="IPipelineInspector"/>.
    /// </summary>
    public interface IPipelineInspectorContext : IContext
    {
        /// <summary>
        /// Gets the proxy factory.
        /// </summary>
        /// <value>The proxy factory.</value>
        IProxyFactory ProxyFactory { get; }

        /// <summary>
        /// Gets the timer strategy.
        /// </summary>
        /// <value>The timer strategy.</value>
        Func<IExecutionTimer> TimerStrategy { get; }

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

        /// <summary>
        /// Gets the runtime policy strategy.
        /// </summary>
        /// <value>The runtime policy strategy.</value>
        Func<RuntimePolicy> RuntimePolicyStrategy { get; }
    }
}