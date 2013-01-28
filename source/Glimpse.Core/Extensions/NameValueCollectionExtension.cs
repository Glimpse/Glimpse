using System.Collections.Generic;
using System.Collections.Specialized;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="NameValueCollection"/>.
    /// </summary>
    public static class NameValueCollectionExtension
    {
        /// <summary>
        /// Converts a <see cref="NameValueCollection"/> to a <c>IDictionary&lt;string, string&gt;</c>.
        /// </summary>
        /// <param name="input">The <see cref="NameValueCollection"/> to convert.</param>
        /// <returns>
        /// A <c>IDictionary&lt;string, string&gt;</c> containing the same key/value pairs as <param name="input">input</param>.
        /// </returns>
        public static IDictionary<string, string> ToDictionary(this NameValueCollection input)
        {
            var result = new Dictionary<string, string>(input.Count);
            foreach (var key in input.Keys)
            {
                var typedKey = key.ToString();
                result.Add(typedKey, input[typedKey]);
            }

            return result;
        } 
    }
}
