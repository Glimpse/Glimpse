using System.Collections.Generic;
using System.Collections.Specialized;

namespace Glimpse.Net.Extentions
{
    internal static class NameValueCollectionExtentions
    {
        internal static IDictionary<string, string> Flatten(this NameValueCollection collection)
        {
            var result = new Dictionary<string, string>();
            foreach (var key in collection.AllKeys)
            {
                //TODO: How can we handle this in a more generic way?
                var keyValue = string.IsNullOrEmpty(key) ? "*--*" : key;
                result.Add(keyValue, collection[keyValue]);
            }

            if (result.Count == 0) return null;

            return result;
        }
    }
}
