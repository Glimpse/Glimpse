using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core2.Extensibility
{
    public class CastleInvocationToAlternateImplementationContextAdapter : IAlternateImplementationContext
    {
        public CastleInvocationToAlternateImplementationContextAdapter(IInvocation invocation, ILogger logger)
        {
            Contract.Requires<ArgumentNullException>(invocation != null, "invocation");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");

            Invocation = invocation;
            Logger = logger;
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