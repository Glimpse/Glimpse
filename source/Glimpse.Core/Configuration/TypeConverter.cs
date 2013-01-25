using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// <c>TypeConverter</c> is a <see cref="ConfigurationConverterBase"/> used to convert between <see cref="String"/> and <see cref="Type"/> instances.
    /// </summary>
    internal class TypeConverter : ConfigurationConverterBase
    {
        /// <summary>
        /// Converts the given <see cref="String"/> to a <see cref="Type"/>, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="Type" /> that represents the converted value.
        /// </returns>
        /// <exception cref="ArgumentException">Throws an exception if <paramref name="value"/> is <c>null</c>, empty, or not a <see cref="String"/>.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var typeName = value as string;

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(string.Format("Invalid Type name '{0}'.", typeName));
            }

            return Type.GetType(typeName, true, true);
        }

        /// <summary>
        /// Converts the given <see cref="Type"/> to a <see cref="String"/>, using the specified context and culture information.
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
            return value.GetType().AssemblyQualifiedName;
        }
    }
}