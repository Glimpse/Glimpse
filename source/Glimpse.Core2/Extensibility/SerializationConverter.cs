using System;
using System.Collections.Generic;

namespace Glimpse.Core2.Extensibility
{
    public abstract class SerializationConverter<T>:ISerializationConverter
    {
        public IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof (T)}; }
        }

        public object Convert(object obj)
        {
            return Convert((T) obj);
        }

        public abstract object Convert(T obj);
    }
}