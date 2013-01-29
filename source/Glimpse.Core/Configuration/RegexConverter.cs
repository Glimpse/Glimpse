using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// <c>RegexConverter</c> is a <see cref="ConfigurationConverterBase"/> used to convert between <see cref="String"/> and <see cref="Regex"/> instances.
    /// </summary>
    internal class RegexConverter : ConfigurationConverterBase
    {
        /// <summary>
        /// Converts the given <see cref="String"/> to a <see cref="Regex"/>, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="Regex" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var regex = value as string;

            return new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Converts the given <see cref="Regex"/> to a <see cref="String"/>, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>
        /// An <see cref="String" /> that represents the converted value.
        /// </returns>
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