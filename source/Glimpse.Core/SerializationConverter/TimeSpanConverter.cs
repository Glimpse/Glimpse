using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="TimeSpan"/> representation's into rounded off millisecond counts.
    /// </summary>
    public class TimeSpanConverter : ISerializationConverter
    {
        /// <summary>
        /// Gets the supported types the converter will be invoked for.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(TimeSpan);
                yield return typeof(TimeSpan?);
            }
        }

        /// <summary>
        /// Converts the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A double of rounded off milliseconds of the length of the time span.</returns>
        public object Convert(object date)
        {
            var converted = date as TimeSpan?;

            if (converted.HasValue)
            {
                return Math.Round(converted.Value.TotalMilliseconds, 2);
            }

            return null;
        }
    }
}
