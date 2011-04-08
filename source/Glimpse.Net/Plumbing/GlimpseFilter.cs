using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Glimpse.Net.Plumbing
{
    public class GlimpseFilter
    {
        public Filter Filter { get; set; }
        public Guid Guid { get; set; }

        public IList<Guid> Store
        {
            get
            {
                var items = HttpContext.Current.Items;
                var store = items[GlimpseConstants.CalledFilters] as IList<Guid>;
                if (store == null) items[GlimpseConstants.CalledFilters] = store = new List<Guid>();

                return store;
            }
        }

        public void LogCall(Guid guid)
        {
            Store.Add(guid);
        }
    }
}