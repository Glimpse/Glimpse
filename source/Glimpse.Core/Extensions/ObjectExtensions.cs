using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToStringOrDefault(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }

        public static Type GetTypeOrNull(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.GetType();
        }
    }
}
