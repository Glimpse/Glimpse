using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    /// <summary>
    /// Can leverage SerializationConverter<T>
    /// </summary>
    public interface ISerializationConverter
    {
        IEnumerable<Type> SupportedTypes { get; }
        IDictionary<string, object> Convert(object obj);
    }
}