using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static T CastAs<T>(this object obj)
        {
            var wrapper = obj as IWrapper<T>;

            if (wrapper != null)
            {
                return wrapper.GetWrappedObject();
            }

            return (T)obj;
        }
    }
}
