using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Provides the ability for metadata to be sliced into the metadata
    /// response for a given tab.
    /// </summary>
    public interface ITabMetadata
    {
        /// <summary>
        /// Gets the key that should be used in the serialized output
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the metadata for a given tab
        /// </summary>
        /// <param name="tab">Tab that might have metadata generated for it.</param>
        /// <returns></returns>
        object GetTabMetadata(ITab tab);
    }
}
