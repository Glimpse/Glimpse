using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class PipelineInspectorContext : IPipelineInspectorContext
    {
        public ILogger Logger { get; set; }
        public IProxyFactory ProxyFactory { get; set; }
        public Func<IExecutionTimer> TimerStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }

        public PipelineInspectorContext(ILogger logger, IProxyFactory proxyFactory, IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(proxyFactory != null, "proxyFactory");
            Contract.Requires<ArgumentNullException>(messageBroker != null, "messageBroker");
            Contract.Requires<ArgumentNullException>(timerStrategy != null, "getTimer");

            Logger = logger;
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
        }
    }
}