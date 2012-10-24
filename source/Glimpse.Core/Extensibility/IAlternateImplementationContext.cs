using System;
using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    public interface IAlternateImplementationContext : IContext
    {
        object Proxy { get; }
        
        object InvocationTarget { get; }
        
        Type TargetType { get; }
        
        object[] Arguments { get; }
        
        Type[] GenericArguments { get; }
        
        MethodInfo Method { get; }
        
        MethodInfo MethodInvocationTarget { get; }
        
        object ReturnValue { get; set; }

        IMessageBroker MessageBroker { get; }

        IProxyFactory ProxyFactory { get; }

        Func<IExecutionTimer> TimerStrategy { get; }

        Func<RuntimePolicy> RuntimePolicyStrategy { get; }
        
        void SetArgumentValue(int index, object value);
        
        object GetArgumentValue(int index);
        
        MethodInfo GetConcreteMethod();
        
        MethodInfo GetConcreteMethodInvocationTarget();
        
        void Proceed();
    }
}