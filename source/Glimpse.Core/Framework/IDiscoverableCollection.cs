using System;
using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to self discover collection content
    /// </summary>
    /// <typeparam name="T">Collection item type</typeparam>
    public interface IDiscoverableCollection<T> : ICollection<T>
    {
        /// <summary>
        /// Gets a value indicating whether [auto discover].
        /// </summary>
        /// <value><c>true</c> if [auto discover]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Indicates whether the system can automatically call Discover()
        /// </remarks>
        bool AutoDiscover { get; }

        /// <summary>
        /// Gets the discovery location.
        /// </summary>
        /// <value>The discovery location.</value>
        string DiscoveryLocation { get; }

        /// <summary>
        /// Ignores the type.
        /// </summary>
        /// <param name="type">The type.</param>
        void IgnoreType(Type type);

        /// <summary>
        /// Discovers this instance. Triggers the loading of content.
        /// </summary>
        void Discover();
    }
}