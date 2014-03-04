using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Provides the ability for metadata to be sliced into the metadata
    /// response for a given tab.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// Gets the key that should be used in the serialized output
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the metadata for a given tab
        /// </summary>
        /// <param name="tab">Tab that might have metadata generated for it.</param>
        /// <returns>The metadata to be used for the given key</returns>
        object GetMetadata(IReadonlyConfiguration configuration);
    }
}