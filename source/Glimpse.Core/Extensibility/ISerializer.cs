using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a provider that can serialize complex objects to JSON.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified object to JSON.
        /// </summary>
        /// <param name="obj">The target to be Serialized.</param>
        /// <returns>Serialized object.</returns>
        string Serialize(object obj);

        /// <summary>
        /// Registers a collection of serialization converters which can conduct custom
        /// transformations on given types when processed.
        /// </summary>
        /// <param name="converters">The converters.</param>
        void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters);
    }
}