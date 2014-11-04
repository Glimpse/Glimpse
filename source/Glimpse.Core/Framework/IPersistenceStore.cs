using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement a store that Glimpse can 
    /// use to persist requests.
    /// </summary>
    public interface IPersistenceStore : IReadOnlyPersistenceStore
    {
        /// <summary>
        /// Saves the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Save(GlimpseRequest request);

        /// <summary>
        /// Saves the specified system metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        void SaveMetadata(IDictionary<string, object> metadata);
    }
}