using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement a service locator
    /// </summary>
    /// <remarks>
    /// Means by which authors can provide alternative implementations to the 
    /// default ones which the system will use by default. Primary extension injection 
    /// mechanism.
    /// </remarks>
    public interface IServiceLocator
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type that this instance should be</typeparam>
        /// <returns>The instance that matches are the input Type.</returns>
        T GetInstance<T>() where T : class;

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>The instance that matches are the input Type.</returns>
        ICollection<T> GetAllInstances<T>() where T : class;
    }
}