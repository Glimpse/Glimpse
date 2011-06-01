using System.Web;
using System.Web.Mvc;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseValueProvider : IValueProvider //, IUnvalidatedValueProvider
    {
        public IValueProvider ValueProvider { get; set; }

        private HttpContextBase context;
        internal HttpContextBase Context
        {
            get { return context ?? new HttpContextWrapper(HttpContext.Current); }
            set { context = value; }
        }

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
                var store = Context.BinderStore();
                store.CurrentProperty.NotFoundIn.Add(ValueProvider);
            }

            //Trace.Write(string.Format("{0}.ContainsPrefix('{1}') = {2}", ValueProvider.GetType().Name, prefix, containsPrefix));
            return containsPrefix;
        }

        public ValueProviderResult GetValue(string key)
        {
            var result = ValueProvider.GetValue(key);

            Log(result);

            //Trace.Write(string.Format("{0}.GetValue('{1}') = {2}", ValueProvider.GetType().Name, key, result == null ? null : result.AttemptedValue));
            return result;
        }

        protected void Log(ValueProviderResult result)
        {
            if (result != null)
            {
                var store = Context.BinderStore();
                store.CurrentProperty.FoundIn = ValueProvider;
                store.CurrentProperty.AttemptedValue = result.AttemptedValue;
                store.CurrentProperty.Culture = result.Culture;
            }
        }
    }

    internal class GlimpseUnvalidatedValueProvider : GlimpseValueProvider, IUnvalidatedValueProvider
    {
        public GlimpseUnvalidatedValueProvider(IValueProvider valueProvider)
            : base(valueProvider)
        {
        }

        public ValueProviderResult GetValue(string key, bool skipValidation)
        {
            var unvalidatedValueProvider = (IUnvalidatedValueProvider) ValueProvider;

            var result = unvalidatedValueProvider.GetValue(key, skipValidation);

            Log(result);

            //Trace.Write(string.Format("{0}.GetValue('{1}') = {2}", ValueProvider.GetType().Name, key, result == null ? null : result.AttemptedValue));
            return result;
        }
    }
}