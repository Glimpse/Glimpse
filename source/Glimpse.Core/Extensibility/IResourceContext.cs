using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IResourceContext</c> provides implementations of <see cref="IResource"/> access to their parameters and the <see cref="IReadOnlyPersistenceStore"/>.
    /// </summary>
    public interface IResourceContext : IContext
    {
        /// <summary>
        /// Gets the parameters that the resource has declared.
        /// </summary>
        /// <value>The parameters.</value>
        IDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Gets the read only persistence store so that historical requests can 
        /// be accessed.
        /// </summary>
        /// <value>The persistence store.</value>
        IReadOnlyPersistenceStore PersistenceStore { get; }
    }
}