using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> abstraction which makes creating converters that deal with one type easier.
    /// </summary>
    /// <typeparam name="T">
    /// The type supported to convert.
    /// </typeparam>
    public abstract class SerializationConverter<T> : ISerializationConverter
    {
        /// <summary>
        /// Gets the supported types the converter will be invoked for.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        public IEnumerable<Type> SupportedTypes
        {
            get { return new[] { typeof(T) }; }
        }

        /// <summary>
        /// Converts the specified object.
        /// </summary>
        /// <param name="obj">The object to transform.</param>
        /// <returns>The new object representation.</returns>
        public object Convert(object obj)
        {
            return Convert((T)obj);
        }

        /// <summary>
        /// Converts the specified object.
        /// </summary>
        /// <param name="obj">The object to transform.</param>
        /// <returns>The new object representation.</returns>
        public abstract object Convert(T obj);
    }
}