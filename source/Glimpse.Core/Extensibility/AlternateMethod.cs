using System;
using System.Reflection;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Extensibility
{
    public abstract class AlternateMethod : IAlternateMethod
    {
        protected AlternateMethod(Type type, string methodName) : this(type.GetMethod(methodName))
        {
        }

        protected AlternateMethod(Type type, string methodName, BindingFlags bindingFlags) : this(type.GetMethod(methodName, bindingFlags))
        {
        }

        protected AlternateMethod(MethodInfo methodToImplement)
        {
            if (methodToImplement == null)
            {
                throw new ArgumentNullException("methodToImplement");
            }

            MethodToImplement = methodToImplement;
        }

        public MethodInfo MethodToImplement { get; private set; }

        public void NewImplementation(IAlternateImplementationContext context)
        {
            TimerResult timerResult;
            if (!context.TryProceedWithTimer(out timerResult))
            {
                return;
            }

            PostImplementation(context, timerResult);
        }

        public abstract void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult);
    }
}