using System;
using System.Collections.Generic;
using System.Globalization;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    public class DateTimeConverter : ISerializationConverter
    {
        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(DateTime);
                yield return typeof(DateTime?);
            }
        }

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