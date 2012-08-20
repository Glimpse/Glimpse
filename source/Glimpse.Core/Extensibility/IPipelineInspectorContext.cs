using System;
using Glimpse.Core;

namespace Glimpse.Core.Extensibility
{
    public interface IPipelineInspectorContext:IContext
    {
        IProxyFactory ProxyFactory { get; }
        Func<IExecutionTimer> TimerStrategy { get; }
        IMessageBroker MessageBroker { get; }
        Func<RuntimePolicy> RuntimePolicyStrategy { get; }
    }
}