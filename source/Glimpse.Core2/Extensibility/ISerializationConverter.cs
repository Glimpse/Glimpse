using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    //TODO: Create ISerializationConverter<T> to simplify?
    public interface ISerializationConverter
    {
        IEnumerable<Type> SupportedTypes { get; }
        IDictionary<string, object> Convert(object obj);
    }
}