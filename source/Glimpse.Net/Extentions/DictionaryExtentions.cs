//TODO: DELETE ME

/*
using System.Collections;
using System.Collections.Generic;

namespace Glimpse.Net.Extentions
{
    public static class DictionaryExtentions
    {
        public static IDictionary<string, string> Flatten(this IDictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, string>();

            if (dictionary == null) return null;

            foreach (var key in dictionary.Keys)
            {
                result.Add(key, dictionary[key].ToString());
            }

            if (result.Count == 0) return null;

            return result;
        }

        public static void Save(this IDictionary dictionary, object key, object value)
        {
            if (dictionary.Contains(key)) return;

            dictionary.Add(key, value);
        }
    }
}
*/
