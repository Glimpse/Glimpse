using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Configuration
{
    public class RegexConverter : ConfigurationConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var regex = value as string;

            return new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var regex = value as Regex;

            if (regex != null)
            {
                return regex.ToString();
            }

            return string.Empty;
        }
    }
}