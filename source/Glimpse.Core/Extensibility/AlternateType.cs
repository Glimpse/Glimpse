using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An abstract <see cref="IAlternateType{T}"/> implementation that handles the most common <c>TryCreate</c> scenarios.
    /// </summary>
    /// <typeparam name="T">The type to retrieve and alternate for.</typeparam>
    public abstract class AlternateType<T> : IAlternateType<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateType{T}" /> class.
        /// </summary>
        /// <param name="proxyFactory">The proxy factory.</param>
        /// <exception cref="System.ArgumentNullException">Throws and exception if <paramref name="proxyFactory"/> is <c>null</c>.</exception>
        protected AlternateType(IProxyFactory proxyFactory)
        {
            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            ProxyFactory = proxyFactory;
        }

        /// <summary>
        /// Gets or sets the proxy factory.
        /// </summary>
        /// <value>
        /// The proxy factory.
        /// </value>
        public IProxyFactory ProxyFactory { get; set; }

        /// <summary>
        /// Gets all methods for the proxy to override.
        /// </summary>
        /// <value>
        /// All methods.
        /// </value>
        public abstract IEnumerable<IAlternateMethod> AllMethods { get; }

        /// <summary>
        /// Tries to create an alternate type.
        /// </summary>
        /// <param name="originalObj">The original object.</param>
        /// <param name="newObj">The new object.</param>
        /// <returns>A proxied implementation of the <paramref name="originalObj"/>.</returns>
        public bool TryCreate(T originalObj, out T newObj)
        {
            return TryCreate(originalObj, out newObj, null, null);
        }

        /// <summary>
        /// Tries to create an alternate type with mixins.
        /// </summary>
        /// <param name="originalObj">The original object.</param>
        /// <param name="newObj">The new object.</param>
        /// <param name="mixins">The mixins.</param>
        /// <returns>
        /// A proxied implementation of the <paramref name="originalObj" />.
        /// </returns>
        public bool TryCreate(T originalObj, out T newObj, IEnumerable<object> mixins)
        {
            return TryCreate(originalObj, out newObj, mixins, null);
        }

        /// <summary>
        /// Tries to create an alternate type with mixins and constructor arguments.
        /// </summary>
        /// <param name="originalObj">The original object.</param>
        /// <param name="newObj">The new object.</param>
        /// <param name="mixins">The mixins.</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns>
        /// A proxied implementation of the <paramref name="originalObj" />.
        /// </returns>
        public virtual bool TryCreate(T originalObj, out T newObj, IEnumerable<object> mixins, object[] constructorArguments)
        {
            var allMethods = AllMethods;

            if (mixins == null)
            {
                mixins = Enumerable.Empty<object>();
            }

            if (ProxyFactory.IsWrapInterfaceEligible<T>(typeof(T)))
            {
                newObj = ProxyFactory.WrapInterface(originalObj, allMethods, mixins);
                return true;
            }

            if (ProxyFactory.IsWrapClassEligible(typeof(T)))
            {
                try
                {
                    newObj = ProxyFactory.WrapClass(originalObj, allMethods, mixins, constructorArguments);
                    return true;
                }
                catch
                {
                    newObj = null;
                    return false;
                }
            }

            newObj = null;
            return false;
        }
    }
}
