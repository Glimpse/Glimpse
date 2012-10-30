using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Extensions
{
    public static class NameValueCollectionExtension
    {
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
