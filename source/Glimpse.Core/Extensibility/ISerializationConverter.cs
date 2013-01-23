using System;
using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a converter that will provide a custom object 
    /// representation for the supported types.
    /// </summary>
    public interface ISerializationConverter
    {
        /// <summary>
        /// Gets the supported types the converter will be invoked for.
        /// </summary>
        /// <value>The supported types.</value>
        IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Converts the specified object.
        /// </summary>
        /// <param name="target">The object to be converted.</param>
        /// <returns>The new object representation.</returns>
        object Convert(object target);
    }
}