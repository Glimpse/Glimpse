using System;
using System.Web;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;

namespace Glimpse.AspNet
{
    public class AspNetFrameworkProvider:IFrameworkProvider
    {
        /// <summary>
        /// Wrapper around HttpContext.Current for testing purposes. Not for public use.
        /// </summary>
        private HttpContextBase context;
        internal HttpContextBase Context
        {
            get { return context ?? new HttpContextWrapper(HttpContext.Current); }
            set { context = value; }
        }

        public IDataStore HttpRequestStore
        {
            get { return new DictionaryDataStoreAdapter(Context.Items);}
        }

        public IDataStore HttpServerStore
        {
            get { return new HttpApplicationStateBaseDataStoreAdapter(Context.Application); }
        }

        public object RuntimeContext
        {
            get { return Context; }
        }

        public Type RuntimeContextType
        {
            get { return typeof (HttpContextBase); }
        }
    }
}
