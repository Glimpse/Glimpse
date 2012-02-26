using System;
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
            if (logger == null) throw new ArgumentNullException("logger");
            if (proxyFactory == null) throw new ArgumentNullException("proxyFactory");
            if (messageBroker == null) throw new ArgumentNullException("messageBroker");
            if (timerStrategy == null) throw new ArgumentNullException("timerStrategy");
            
            Logger = logger;
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
        }
    }
}