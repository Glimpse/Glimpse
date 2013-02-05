using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An adapter between Castle DynamicProxy's <see cref="IInvocation"/> and Glimpse's <see cref="IAlternateMethodContext"/>.
    /// </summary>
    public class CastleInvocationToAlternateMethodContextAdapter : IAlternateMethodContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CastleInvocationToAlternateMethodContextAdapter" /> class.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="proxyFactory">The proxy factory.</param>
        /// <param name="timerStrategy">The timer strategy.</param>
        /// <param name="runtimePolicyStrategy">The runtime policy strategy.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameter is <c>null</c>.</exception>
        public CastleInvocationToAlternateMethodContextAdapter(IInvocation invocation, ILogger logger, IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            if (invocation  == null)
            {
                throw new ArgumentNullException("invocation");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (messageBroker == null)
            {
                throw new ArgumentNullException("messageBroker");
            }

            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            if (timerStrategy == null)
            {
                throw new ArgumentNullException("timerStrategy");
            }

            if (runtimePolicyStrategy == null)
            {
                throw new ArgumentNullException("runtimePolicyStrategy");
            }

            Invocation = invocation;
            Logger = logger;
            MessageBroker = messageBroker;
            ProxyFactory = proxyFactory;
            TimerStrategy = timerStrategy;
            RuntimePolicyStrategy = runtimePolicyStrategy;
        }

        /// <summary>
        /// Gets or sets the invocation.
        /// </summary>
        /// <value>
        /// The invocation.
        /// </value>
        public IInvocation Invocation { get; set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets the proxy object on which the alternate implementation is invoked.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public object Proxy
        {
            get { return Invocation.Proxy; }
        }

        /// <summary>
        /// Gets the object on which the invocation is performed. This is different from proxy object
        /// because most of the time this will be the proxy target object.
        /// </summary>
        /// <value>
        /// The invocation target.
        /// </value>
        public object InvocationTarget
        {
            get { return Invocation.InvocationTarget; }
        }

        /// <summary>
        /// Gets the type of the target object for the intercepted method.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType
        {
            get { return Invocation.TargetType; }
        }

        /// <summary>
        /// Gets the arguments that <see cref="IAlternateMethod" /> has been invoked with.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public object[] Arguments
        {
            get { return Invocation.Arguments; }
        }

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <value>
        /// The generic arguments.
        /// </value>
        public Type[] GenericArguments
        {
            get { return Invocation.GenericArguments; }
        }

        /// <summary>
        /// Gets the method representing the method being invoked on the proxy.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public MethodInfo Method
        {
            get { return Invocation.Method; }
        }

        /// <summary>
        /// Gets the method info on the target class.
        /// </summary>
        /// <value>
        /// The method invocation target.
        /// </value>
        public MethodInfo MethodInvocationTarget
        {
            get { return Invocation.MethodInvocationTarget; }
        }

        /// <summary>
        /// Gets or sets the return value of the method.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        public object ReturnValue
        {
            get { return Invocation.ReturnValue; }
            set { Invocation.ReturnValue = value; }
        }

        /// <summary>
        /// Gets the message broker.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; private set; }

        /// <summary>
        /// Gets the proxy factory.
        /// </summary>
        /// <value>
        /// The proxy factory.
        /// </value>
        public IProxyFactory ProxyFactory { get; private set; }

        /// <summary>
        /// Gets the timer strategy.
        /// </summary>
        /// <value>
        /// The timer strategy.
        /// </value>
        public Func<IExecutionTimer> TimerStrategy { get; private set; }

        /// <summary>
        /// Gets the runtime policy strategy.
        /// </summary>
        /// <value>
        /// The runtime policy strategy.
        /// </value>
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; private set; }

        /// <summary>
        /// Overrides the value of an argument at the given index with the new value provided.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void SetArgumentValue(int index, object value)
        {
            Invocation.SetArgumentValue(index, value);
        }

        /// <summary>
        /// Gets the value of the argument at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// Value of the argument.
        /// </returns>
        public object GetArgumentValue(int index)
        {
            return Invocation.GetArgumentValue(index);
        }

        /// <summary>
        /// Gets the concrete method.
        /// </summary>
        /// <returns>
        /// The method info of the method that is the proxy that is the alternative implementation.
        /// </returns>
        public MethodInfo GetConcreteMethod()
        {
            return Invocation.GetConcreteMethod();
        }

        /// <summary>
        /// Gets the concrete method invocation target.
        /// </summary>
        /// <returns>
        /// The method info of the method that is the target of the alternative implementation.
        /// </returns>
        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            return Invocation.GetConcreteMethodInvocationTarget();
        }

        /// <summary>
        /// Proceeds the call to the next implementation in line, and ultimately to the target method.
        /// </summary>
        public void Proceed()
        {
            Invocation.Proceed();
        }
    }
}