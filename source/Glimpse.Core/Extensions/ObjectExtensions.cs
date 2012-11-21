using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
