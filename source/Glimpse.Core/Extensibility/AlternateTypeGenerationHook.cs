using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of Castle DynamicProxy's <see cref="IProxyGenerationHook"/> for Glimpse.
    /// </summary>
    /// <typeparam name="T">The type being proxied.</typeparam>
    public class AlternateTypeGenerationHook<T> : IProxyGenerationHook
    {
        private IEnumerable<MethodInfo> methodSet;
        
        private IEnumerable<IAlternateMethod> methodImplementations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateTypeGenerationHook{T}" /> class.
        /// </summary>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if either <paramref name="methodImplementations"/> or <paramref name="logger"/> are <c>null</c>.</exception>
        public AlternateTypeGenerationHook(IEnumerable<IAlternateMethod> methodImplementations, ILogger logger)
        {
            if (methodImplementations == null)
            {
                throw new ArgumentNullException("methodImplementations");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            
            MethodImplementations = methodImplementations;
            Logger = logger;
        }

        /// <summary>
        /// Gets or sets the method implementations.
        /// </summary>
        /// <value>
        /// The method implementations.
        /// </value>
        internal IEnumerable<IAlternateMethod> MethodImplementations
        {
            get
            {
                return methodImplementations;
            }

            set
            {
                methodImplementations = value;
                methodSet = value.Select(m => m.MethodToImplement);
            }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        internal ILogger Logger { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var input = obj as AlternateTypeGenerationHook<T>;

            if (input == null)
            {
                return false;
            }

            var result = methodSet.SequenceEqual(input.MethodImplementations.Select(m => m.MethodToImplement));

            return result;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap
            unchecked 
            {
                int hash = 17;

                foreach (var implementation in MethodImplementations)
                {
                    hash = (hash * 23) + implementation.GetType().GetHashCode();
                }

                return hash;
            }
        }

        /// <summary>
        /// Invoked by the generation process to notify that the whole process has completed.
        /// </summary>
        public void MethodsInspected()
        {
            Logger.Debug(Resources.AlternateTypeGenerationHookMethodsInspected, typeof(T));
        }

        /// <summary>
        /// Invoked by the generation process to notify that a member was not marked as virtual.
        /// </summary>
        /// <param name="type">The type which declares the non-virtual member.</param>
        /// <param name="memberInfo">The non-virtual member.</param>
        /// <remarks>
        /// This method gives an opportunity to inspect any member which is not proxyable of a type that has
        /// been requested to be proxied, and if appropriate - throw an exception to notify the caller.
        /// </remarks>
        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            Logger.Debug(Resources.AlternateTypeGenerationHookNonProxyableMemberNotification, memberInfo.Name, type);
        }

        /// <summary>
        /// Invoked by the generation process to determine if the specified method should be proxied.
        /// </summary>
        /// <param name="type">The type which declares the given method.</param>
        /// <param name="methodInfo">The method to inspect.</param>
        /// <returns>
        /// True if the given method should be proxied; false otherwise.
        /// </returns>
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return MethodImplementations.Any(i => i.MethodToImplement == methodInfo);
        }
    }
}