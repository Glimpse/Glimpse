using System;
using System.Reflection;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the context used by <see cref="IAlternateMethod"/>
    /// </summary>
    public interface IAlternateMethodContext : IContext
    {
        /// <summary>
        /// Gets the proxy object on which the alternate implementation is invoked.
        /// </summary>
        /// <value>The proxy.</value>
        object Proxy { get; }

        /// <summary>
        /// Gets the object on which the invocation is performed. This is different from proxy object 
        /// because most of the time this will be the proxy target object.
        /// </summary>
        /// <value>The invocation target.</value>
        object InvocationTarget { get; }

        /// <summary>
        /// Gets the type of the target object for the intercepted method.
        /// </summary>
        /// <value>The type of the target.</value>
        Type TargetType { get; }

        /// <summary>
        /// Gets the arguments that <see cref="IAlternateMethod"/> has been invoked with.
        /// </summary>
        /// <value>The arguments.</value>
        object[] Arguments { get; }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <value>The generic arguments.</value>
        Type[] GenericArguments { get; }

        /// <summary>
        /// Gets the method representing the method being invoked on the proxy.
        /// </summary>
        /// <value>The method.</value>
        MethodInfo Method { get; }

        /// <summary>
        /// Gets the method info on the target class.
        /// </summary>
        /// <value>The method invocation target.</value>
        MethodInfo MethodInvocationTarget { get; }

        /// <summary>
        /// Gets or sets the return value of the method.
        /// </summary>
        /// <value>The return value.</value>
        object ReturnValue { get; set; }

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>The message broker.</value>
        IMessageBroker MessageBroker { get; }

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
        /// Gets the runtime policy strategy.
        /// </summary>
        /// <value>The runtime policy strategy.</value>
        Func<RuntimePolicy> RuntimePolicyStrategy { get; }

        /// <summary>
        /// Overrides the value of an argument at the given index with the new value provided.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void SetArgumentValue(int index, object value);

        /// <summary>
        /// Gets the value of the argument at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Value of the argument.</returns>
        object GetArgumentValue(int index);

        /// <summary>
        /// Gets the concrete method.
        /// </summary>
        /// <returns>The method info of the method that is the proxy that is the alternative implementation.</returns>
        MethodInfo GetConcreteMethod();

        /// <summary>
        /// Gets the concrete method invocation target.
        /// </summary>
        /// <returns>The method info of the method that is the target of the alternative implementation.</returns>
        MethodInfo GetConcreteMethodInvocationTarget();

        /// <summary>
        /// Proceeds the call to the next implementation in line, and ultimately to the target method.
        /// </summary>
        void Proceed();
    }
}