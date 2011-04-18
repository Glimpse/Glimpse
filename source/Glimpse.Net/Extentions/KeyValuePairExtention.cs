using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Net.Extentions
{
    internal static class KeyValuePairExtention
    {
        internal static IDictionary<string, object> Flatten(this IEnumerable<KeyValuePair<string, object>> source)
        {
            return source.ToDictionary<KeyValuePair<string, object>, string, object>(item => item.Key,
                                                                                     item => item.Value.ToString());
        }
    }
}
