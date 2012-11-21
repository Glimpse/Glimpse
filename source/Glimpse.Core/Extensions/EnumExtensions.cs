using System;
using System.ComponentModel;

namespace Glimpse.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumeration)
        {
            var enumValue = enumeration.ToString();
            var enumType = enumeration.GetType();

            var attribute = enumType.GetField(enumValue).GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attribute == null || attribute.Length == 0)
            {
                return string.Empty;
            }

            return attribute[0].Description;
        }
    }
}