using System;
using System.Collections.Generic;
using System.Globalization;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="DateTime"/> representation's into culturally invariant strings.
    /// </summary>
    public class DateTimeConverter : ISerializationConverter
    {
        /// <summary>
        /// Gets the supported types the converter will be invoked for.
        /// </summary>
        /// <value>
        /// The supported types: <see cref="DateTime"/> and <see cref="Nullable{DateTime}"/>, where <c>T</c> is a <see cref="DateTime"/>.
        /// </value>
        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(DateTime);
                yield return typeof(DateTime?);
            }
        }

        /// <summary>
        /// Converts the specified date into a culturally invariant string.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A culturally invariant date and time string</returns>
        public object Convert(object date)
        {
            var converted = date as DateTime?;

            if (converted.HasValue)
            {
                return converted.Value.ToString(CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}