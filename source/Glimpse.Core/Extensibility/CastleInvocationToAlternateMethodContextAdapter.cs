using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    public class CastleInvocationToAlternateMethodContextAdapter : IAlternateMethodContext
    {
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

        public IInvocation Invocation { get; set; }
        
        public ILogger Logger { get; set; }

        public object Proxy
        {
            get { return Invocation.Proxy; }
        }

        public object InvocationTarget
        {
            get { return Invocation.InvocationTarget; }
        }

        public Type TargetType
        {
            get { return Invocation.TargetType; }
        }

        public object[] Arguments
        {
            get { return Invocation.Arguments; }
        }

        public Type[] GenericArguments
        {
            get { return Invocation.GenericArguments; }
        }

        public MethodInfo Method
        {
            get { return Invocation.Method; }
        }

        public MethodInfo MethodInvocationTarget
        {
            get { return Invocation.MethodInvocationTarget; }
        }

        public object ReturnValue
        {
            get { return Invocation.ReturnValue; }
            set { Invocation.ReturnValue = value; }
        }

        public IMessageBroker MessageBroker { get; private set; }
        
        public IProxyFactory ProxyFactory { get; private set; }
        
        public Func<IExecutionTimer> TimerStrategy { get; private set; }
        
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; private set; }

        public void SetArgumentValue(int index, object value)
        {
            Invocation.SetArgumentValue(index, value);
        }

        public object GetArgumentValue(int index)
        {
            return Invocation.GetArgumentValue(index);
        }

        public MethodInfo GetConcreteMethod()
        {
            return Invocation.GetConcreteMethod();
        }

        public MethodInfo GetConcreteMethodInvocationTarget()
        {
            return Invocation.GetConcreteMethodInvocationTarget();
        }

        public void Proceed()
        {
            Invocation.Proceed();
        }
    }
}