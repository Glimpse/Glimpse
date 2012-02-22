using System;
using System.Reflection;

namespace Glimpse.Core2.Extensibility
{
    public interface IAlternateImplementationContext : IContext
    {
        //TODO: Rename any of these items if there is a better name
        object Proxy { get; }
        object InvocationTarget { get; }
        Type TargetType { get; }
        object[] Arguments { get; }
        Type[] GenericArguments { get; }
        MethodInfo Method { get; }
        MethodInfo MethodInvocationTarget { get; }
        object ReturnValue { get; set; }
        void SetArgumentValue(int index, object value);
        object GetArgumentValue(int index);
        MethodInfo GetConcreteMethod();
        MethodInfo GetConcreteMethodInvocationTarget();
        void Proceed();
    }
}