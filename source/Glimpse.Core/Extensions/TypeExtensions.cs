using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Extensions
{
    public static class TypeExtensions
    {
        public static string ConvertToSafeJson(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return type.FullName.Replace('.', '_');
        }
    }
}
