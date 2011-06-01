using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseFilter
    {
        public Filter Filter { get; set; }

        private HttpContextBase context;
        internal HttpContextBase Context
        {
            get { return context ?? new HttpContextWrapper(HttpContext.Current); }
            set { context = value; }
        }

        public IList<GlimpseFilterCalledMetadata> Store
        {
            get
            {
                var items = Context.Items;
                var store = items[GlimpseConstants.CalledFilters] as IList<GlimpseFilterCalledMetadata>;
                if (store == null) items[GlimpseConstants.CalledFilters] = store = new List<GlimpseFilterCalledMetadata>();

                return store;
            }
        }

        public GlimpseFilterCalledMetadata LogCall(Guid guid)
        {
            var metadata = new GlimpseFilterCalledMetadata{Guid = guid};
            Store.Add(metadata);
            return metadata;
        }
    }
}