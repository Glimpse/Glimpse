using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Extensions
{
    public static class KeyValuePairExtension
    {
        public static IDictionary<string, object> Flatten(this IEnumerable<KeyValuePair<string, object>> source)
        {
            return source.ToDictionary<KeyValuePair<string, object>, string, object>(item => item.Key,
                                                                                     item => item.Value.ToString());
        }
    }
}
