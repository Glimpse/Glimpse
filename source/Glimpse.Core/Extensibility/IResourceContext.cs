using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the resource context that is used when a resource 
    /// is being executed.
    /// </summary>
    public interface IResourceContext : IContext
    {
        /// <summary>
        /// Gets the parameters that the resource is allowed to access.
        /// </summary>
        /// <remarks>
        /// Typically carries the target URI query string parameters.
        /// </remarks>
        /// <value>The parameters.</value>
        IDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Gets the persistence store so that historical requests can 
        /// be accessed.
        /// </summary>
        /// <value>The persistence store.</value>
        IReadOnlyPersistenceStore PersistenceStore { get; }
    }
}