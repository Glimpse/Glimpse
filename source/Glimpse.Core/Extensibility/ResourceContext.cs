using System.Collections.Generic;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The implementation of <see cref="IResourceContext"/>, used in the <c>Execute</c> method of <see cref="IResource"/>.
    /// </summary>
    public class ResourceContext : IResourceContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceContext" /> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="persistenceStore">The persistence store.</param>
        /// <param name="logger">The logger.</param>
        public ResourceContext(IDictionary<string, string> parameters, IReadOnlyPersistenceStore persistenceStore, ILogger logger)
        {
            Parameters = parameters;
            PersistenceStore = persistenceStore;
            Logger = logger;
        }

        /// <summary>
        /// Gets or sets the parameters that the resource has declared.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the read only persistence store so that historical requests can
        /// be accessed.
        /// </summary>
        /// <value>
        /// The persistence store.
        /// </value>
        public IReadOnlyPersistenceStore PersistenceStore { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        public ILogger Logger { get; set; }
    }
}