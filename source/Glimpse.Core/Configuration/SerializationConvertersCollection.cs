using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// Represents a collection of <see cref="ISerializationConverter"/>
    /// </summary>
    public class SerializationConvertersCollection : DiscoverableCollection<ISerializationConverter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationConvertersCollection" /> class
        /// </summary>
        /// <param name="collectionSettings">The collection settings</param>
        /// <param name="logger">The logger</param>
        public SerializationConvertersCollection(CollectionSettings collectionSettings, ILogger logger)
            : base(collectionSettings, logger)
        {
        }
    }
}