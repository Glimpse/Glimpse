using System.Collections.Generic;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value, if available, or <param name="ifNotFound">ifNotFound</param>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The item key.</param>
        /// <param name="ifNotFound">The fallback value.</param>
        /// <returns>
        /// Returns the item in <param name="dictionary">dictionary</param> that matches <param name="key">key</param>, falling back to the value of <param name="ifNotFound">ifNotFound</param> if the item is unavailable.
        /// </returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue ifNotFound = default(TValue))
        {
            TValue val;
            return dictionary.TryGetValue(key, out val) ? val : ifNotFound;
        }
    }
}
