using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IPipelineInspectorContext:IContext
    {
        IProxyFactory ProxyFactory { get; }
        Func<IExecutionTimer> TimerStrategy { get; }
        IMessageBroker MessageBroker { get; }
    }
}