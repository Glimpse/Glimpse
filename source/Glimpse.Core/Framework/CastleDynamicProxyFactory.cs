using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// An implementation of <see cref="IProxyFactory"/> which leverages <see href="http://www.castleproject.org/projects/dynamicproxy/">Castle DynamicProxy</see>.
    /// </summary>
    public class CastleDynamicProxyFactory : IProxyFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CastleDynamicProxyFactory" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="messageBroker">The message broker.</param>
        /// <param name="timerStrategy">The timer strategy.</param>
        /// <param name="runtimePolicyStrategy">The runtime policy strategy.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if any parameter if <c>null</c>.</exception>
        public CastleDynamicProxyFactory(ILogger logger, IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (messageBroker == null)
            {
                throw new ArgumentNullException("messageBroker");
            }

            if (timerStrategy == null)
            {
                throw new ArgumentNullException("timerStrategy");
            }

            if (runtimePolicyStrategy == null)
            {
                throw new ArgumentNullException("runtimePolicyStrategy");
            }

            Logger = logger;
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
            RuntimePolicyStrategy = runtimePolicyStrategy;
            ProxyGenerator = new ProxyGenerator();
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the proxy generator.
        /// </summary>
        /// <value>
        /// The proxy generator.
        /// </value>
        public ProxyGenerator ProxyGenerator { get; set; }

        /// <summary>
        /// Gets or sets the message broker.
        /// </summary>
        /// <value>
        /// The message broker.
        /// </value>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the runtime policy strategy.
        /// </summary>
        /// <value>
        /// The runtime policy strategy.
        /// </value>
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

        /// <summary>
        /// Gets or sets the timer strategy.
        /// </summary>
        /// <value>
        /// The timer strategy.
        /// </value>
        public Func<IExecutionTimer> TimerStrategy { get; set; }

        /// <summary>
        /// Determines whether the specified type is eligible to be interface wrapped.
        /// </summary>
        /// <typeparam name="TToWrap">The type to wrap.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if type is eligible for interface wrapping; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public bool IsWrapInterfaceEligible<TToWrap>(Type type)
        {
            if (!typeof(TToWrap).IsInterface)
            {
                return false;
            }

            if (!type.IsAssignableFrom(typeof(TToWrap)))
            {
                return false;
            }

            return IsGenerallyEligable(type);
        }

        /// <summary>
        /// Wraps the interface.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>
        /// Wrapped instance.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return WrapInterface(instance, methodImplementations, Enumerable.Empty<object>());
        }

        /// <summary>
        /// Wraps the interface.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>
        /// Wrapped instance.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public T WrapInterface<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            CheckInput(instance, methodImplementations);

            var options = CreateProxyOptions<T>(methodImplementations, mixins ?? Enumerable.Empty<object>());
            var wrapper = new CastleDynamicProxyWrapper<T>();
            options.AddMixinInstance(wrapper);

            var interceptorArray = CreateInterceptorArray(methodImplementations);

            var result = ProxyGenerator.CreateInterfaceProxyWithTarget(instance, options, interceptorArray);

            wrapper.ProxyTargetAccessor = result as IProxyTargetAccessor;

            return result;
        }

        /// <summary>
        /// Determines whether the specified type eligible to be class wrapped.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type of eligible for class wrapping; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public bool IsWrapClassEligible(Type type)
        {
            return IsExtendClassEligible(type);
        }

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>
        /// Wrapped instance.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return WrapClass<T>(instance, methodImplementations, Enumerable.Empty<object>());
        }

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>
        /// Wrapped instance.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            return WrapClass<T>(instance, methodImplementations, mixins, null);
        }

        /// <summary>
        /// Wraps the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns>
        /// Wrapped instance.
        /// </returns>
        /// <remarks>
        /// Wrapping takes a target instance, generates a new type that extends
        /// the input types and injects the target object within the new instance.
        /// </remarks>
        public T WrapClass<T>(T instance, IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class
        {
            CheckInput(instance, methodImplementations);

            var options = CreateProxyOptions<T>(methodImplementations, mixins ?? Enumerable.Empty<object>());
            var wrapper = new CastleDynamicProxyWrapper<T>();
            options.AddMixinInstance(wrapper);

            var interceptorArray = CreateInterceptorArray(methodImplementations);

            var result = (T)ProxyGenerator.CreateClassProxyWithTarget(typeof(T), instance, options, ToArrayOrDefault(constructorArguments), interceptorArray);

            wrapper.ProxyTargetAccessor = result as IProxyTargetAccessor;

            return result;
        }

        /// <summary>
        /// Determines whether the specified type is eligible to be extended.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type is elibible to be extended; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExtendClassEligible(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            return IsGenerallyEligable(type);
        }

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <returns>
        /// Extended instance.
        /// </returns>
        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations) where T : class
        {
            return ExtendClass<T>(methodImplementations, Enumerable.Empty<object>());
        }

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>
        /// Extended instance.
        /// </returns>
        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins) where T : class
        {
            return ExtendClass<T>(methodImplementations, mixins, null);
        }

        /// <summary>
        /// Extends the class.
        /// </summary>
        /// <typeparam name="T">Instance type</typeparam>
        /// <param name="methodImplementations">The method implementations.</param>
        /// <param name="mixins">The mixins.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns>
        /// Extended instance.
        /// </returns>
        public T ExtendClass<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins, IEnumerable<object> constructorArguments) where T : class
        {
            CheckInput(methodImplementations);

            var options = CreateProxyOptions<T>(methodImplementations, mixins ?? Enumerable.Empty<object>());
            var interceptorArray = CreateInterceptorArray(methodImplementations);

            return (T)ProxyGenerator.CreateClassProxy(typeof(T), options, ToArrayOrDefault(constructorArguments), interceptorArray);
        }

        private static TSource[] ToArrayOrDefault<TSource>(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.ToArray();
        }

        private void CheckInput(IEnumerable<IAlternateMethod> methodImplementations)
        {
            if (methodImplementations == null)
            {
                throw new ArgumentNullException("methodImplementations");
            }
        }

        private void CheckInput(object instance, IEnumerable<IAlternateMethod> methodImplementations)
        {
            CheckInput(methodImplementations);

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
        }

        private bool IsGenerallyEligable(Type type)
        { 
            return !type.IsSealed && !type.IsAssignableFrom(typeof(IProxyTargetAccessor));
        }

        private ProxyGenerationOptions CreateProxyOptions<T>(IEnumerable<IAlternateMethod> methodImplementations, IEnumerable<object> mixins)
        {
            var generationHook = new AlternateTypeGenerationHook<T>(methodImplementations, Logger);
            var selector = new AlternateTypeSelector();
            var options = new ProxyGenerationOptions(generationHook) { Selector = selector };

            if (mixins != null)
            {
                foreach (var mixin in mixins)
                {
                    options.AddMixinInstance(mixin);
                }
            }

            return options;
        }

        private IInterceptor[] CreateInterceptorArray(IEnumerable<IAlternateMethod> methodImplementations)
        {
            return (from implementaion in methodImplementations select new AlternateTypeToCastleInterceptorAdapter(implementaion, Logger, MessageBroker, this, TimerStrategy, RuntimePolicyStrategy)).ToArray();
        }
    }
}