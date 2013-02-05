using System;
using System.Reflection;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An abstract <see cref="IAlternateMethod"/> implementation which handles checking Glimpse policies and timing original implementations.
    /// </summary>
    public abstract class AlternateMethod : IAlternateMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateMethod" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        protected AlternateMethod(Type type, string methodName) : this(type.GetMethod(methodName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateMethod" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        protected AlternateMethod(Type type, string methodName, BindingFlags bindingFlags) : this(type.GetMethod(methodName, bindingFlags))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateMethod" /> class.
        /// </summary>
        /// <param name="methodToImplement">The method to implement.</param>
        /// <exception cref="System.ArgumentNullException">methodToImplement</exception>
        protected AlternateMethod(MethodInfo methodToImplement)
        {
            if (methodToImplement == null)
            {
                throw new ArgumentNullException("methodToImplement");
            }

            MethodToImplement = methodToImplement;
        }

        /// <summary>
        /// Gets the method to implement.
        /// </summary>
        /// <value>
        /// The method to implement.
        /// </value>
        /// <remarks>
        /// The info of the method that this alternate is for.
        /// </remarks>
        public MethodInfo MethodToImplement { get; private set; }

        /// <summary>
        /// New implementation that is called in-place of the the original method.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// It is up to this method to call the underlying target method.
        /// </remarks>
        public void NewImplementation(IAlternateMethodContext context)
        {
            TimerResult timerResult;
            if (!context.TryProceedWithTimer(out timerResult))
            {
                return;
            }

            PostImplementation(context, timerResult);
        }

        /// <summary>
        /// Additional code to be executed after the original implementation has been run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timerResult">The timer result.</param>
        public abstract void PostImplementation(IAlternateMethodContext context, TimerResult timerResult);
    }
}