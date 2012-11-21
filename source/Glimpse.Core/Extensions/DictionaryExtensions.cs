using System.Collections.Generic;

namespace Glimpse.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> input, TKey key, TValue ifNotFound = default(TValue))
        {
            TValue val;
            return input.TryGetValue(key, out val) ? val : ifNotFound;
        }
    }
}
