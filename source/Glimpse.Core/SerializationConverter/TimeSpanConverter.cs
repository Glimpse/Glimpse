using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.SerializationConverter
{
    public class TimeSpanConverter : ISerializationConverter
    {
        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(TimeSpan);
                yield return typeof(TimeSpan?);
            }
        }

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
