using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Extensions;

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

            if (!containsPrefix)
            {
                var store = HttpContext.Current.BinderStore();
                store.CurrentProperty.NotFoundIn.Add(ValueProvider);
            }

            Trace.Write(string.Format("{0}.ContainsPrefix('{1}') = {2}", ValueProvider.GetType().Name, prefix, containsPrefix));
            return containsPrefix;
        }

        public ValueProviderResult GetValue(string key)
        {
            var k = key;
            var result = ValueProvider.GetValue(k);

            if (result != null)
            {
                var store = HttpContext.Current.BinderStore();
                store.CurrentProperty.FoundIn = ValueProvider;
                store.CurrentProperty.RawValue = result.RawValue;
                store.CurrentProperty.AttemptedValue = result.AttemptedValue;
                store.CurrentProperty.Culture = result.Culture;
            }

            Trace.Write(string.Format("{0}.GetValue('{1}') = {2}", ValueProvider.GetType().Name, key, result == null ? null : result.AttemptedValue));
            return result;
        }
    }
}