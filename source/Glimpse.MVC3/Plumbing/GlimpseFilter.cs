using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseFilter
    {
        public Filter Filter { get; set; }

        public IList<GlimpseFilterCalledMetadata> Store
        {
            get
            {
                var items = HttpContext.Current.Items;
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