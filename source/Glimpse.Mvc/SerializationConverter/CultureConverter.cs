using System;
using System.Globalization;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.SerializationConverter
{
    public class CultureConverter : SerializationConverter<CultureInfo>
    {
        public override object Convert(CultureInfo culture)
        {
            return culture.DisplayName;
        }
    }
}