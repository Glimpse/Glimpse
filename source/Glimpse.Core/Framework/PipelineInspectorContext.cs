using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class PipelineInspectorContext : IPipelineInspectorContext
    {
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

        public ILogger Logger { get; set; }
        
        public IProxyFactory ProxyFactory { get; set; }
        
        public Func<IExecutionTimer> TimerStrategy { get; set; }
        
        public IMessageBroker MessageBroker { get; set; }
        
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
    }
}