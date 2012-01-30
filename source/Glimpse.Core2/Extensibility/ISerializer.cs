using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public interface ISerializer
    {
        string Serialize(object obj);
        void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters);
    }
}