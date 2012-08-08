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
        object Convert(object obj);
    }
}