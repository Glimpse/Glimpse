using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// Definition for a converter that will provide a custom object 
    /// representation to serialize for the supported types.
    /// </para>
    /// <para>
    /// When implemented, a serialization converter will be discovered and added to the collection of serialization converters. 
    /// </para>
    /// </summary>
    public interface ISerializationConverter
    {
        /// <summary>
        /// Gets the supported types the converter will be invoked for.
        /// </summary>
        /// <value>The supported types.</value>
        IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Converts the specified object into a representation suitable for serialization.
        /// </summary>
        /// <param name="target">The object to be converted.</param>
        /// <returns>The new object representation.</returns>
        object Convert(object target);
    }
}