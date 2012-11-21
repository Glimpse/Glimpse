using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Can leverage SerializationConverter&lt;T&gt;
    /// </summary>
    public interface ISerializationConverter
    {
        IEnumerable<Type> SupportedTypes { get; }
        
        object Convert(object obj);
    }
}