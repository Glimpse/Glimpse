using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public class SerializationConvertersCollection : DiscoverableCollection<ISerializationConverter>
    {
        public SerializationConvertersCollection(
            CollectionSettings configuration, 
            ILogger logger) : base(configuration, logger)
        {
        }
    }
}
