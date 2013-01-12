using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// Definition of a factory tha can create proxies of given objects/types. Factory 
    /// supports wrapping a interfaces, wrapping a class or extending a class. 
    /// </para>
    /// <para>
    /// Wrapping takes a target instance, generates a new type that extends the input 
    /// types and injects the target object within the new instance. Extends takes a 
    /// type and generates a new type that extends it.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// When selecting a class to wrap or extend, it is important to know what constructors
    /// you have and whether a default constructor is available.
    /// </para>
    /// <para>
    /// When a method is called on a wrapped target, it is important to note that once the 
    /// targets version of that method is called, no other method on the proxy will be called.
    /// This can become important if you are try to wrap multiple methods where one calls the 
    /// other.
    /// </para>
    /// <para>
    /// When a method is called on a extended class, it is important to note that the new 
    /// class will behave differently to that of a wrapped classes. Meaning that if you have 
    /// multiple methods that you are providing alternates for, if one calls the other, the 
    /// alternate will be called on both methods.
    /// </para>
    /// </remarks>
    public interface IProxyFactory
    {
        /// <summary>
        /// Determines whether the specified type is wrap interface eligible. 
        /// </summary>
        /// <typeparam name="TToWrap">The type of the T to wrap.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is wrap interface eligible] [the specified type]; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        bool IsWrapInterfaceEligible<TToWrap>(Type type);

        /// <summary>
        /// Wraps the interface. 
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>Wrapped instance.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        /// <summary>
        /// Wraps the interface.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>Wrapped instance.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        /// <summary>
        /// Determines whether the specified type is wrap class eligible.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is wrap class eligible] [the specified type]; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        bool IsWrapClassEligible(Type type);

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>Wrapped instance.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>Wrapped instance.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns>Wrapped instance.</returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class;

        /// <summary>
        /// Determines whether the specified type is extend class eligible.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is extend class eligible] [the specified type]; otherwise, <c>false</c>.</returns>
        bool IsExtendClassEligible(Type type);

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>Extended instance.</returns>
        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations) where T : class;

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>Extended instance.</returns>
        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class;

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns>Extended instance.</returns>
        T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class;
    }
}