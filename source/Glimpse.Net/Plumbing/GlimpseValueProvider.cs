using System.Diagnostics;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseValueProvider:IValueProvider
    {
        public IValueProvider ValueProvider { get; set; }

        public GlimpseValueProvider(IValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }

        public bool ContainsPrefix(string prefix)
        {
            var p = prefix;
            var containsPrefix = ValueProvider.ContainsPrefix(p);
            Trace.Write(string.Format("{0}.ContainsPrefix('{1}') = {2}", ValueProvider.GetType().Name, prefix, containsPrefix));
            return containsPrefix;
        }

        public ValueProviderResult GetValue(string key)
        {
            var k = key;
            var result = ValueProvider.GetValue(k);
            Trace.Write(string.Format("{0}.GetValue('{1}') = {2}", ValueProvider.GetType().Name, key, result == null ? null : result.AttemptedValue));
            return result;
        }
    }
}