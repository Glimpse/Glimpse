using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.AspNet.Support
{
    public static class Serialization
    {
        public static object GetValueSafe(object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                if (!type.IsSerializable)
                {
                    if (type.GetMethod("ToString").DeclaringType == type)
                    {
                        value = value.ToString();
                    }
                    else
                    {
                        value = @"\Non serializable type :(\";
                    }
                }
            }

            return value;
        }
    }
}
