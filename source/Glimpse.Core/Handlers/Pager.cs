using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class Pager : JsonHandlerBase
    {
        [ImportMany]
        public IEnumerable<IGlimpsePlugin> Plugins { get; set; }

        [ImportingConstructor]
        public Pager(GlimpseSerializer serializer) : base(serializer)
        {
        }

        public override string ResourceName
        {
            get { return "Pager"; }
        }

        protected override object GetData(HttpContextBase context)
        {
            var key = Guid.Parse(context.Request.QueryString["key"]);
            var plugin = Plugins.OfType<IProvideGlimpsePaging>().FirstOrDefault(p => p.PagerKey == key);
            if (plugin == null)
                return new { Error = true, Message = string.Format("No paging plugin with key '{0}' found.", key) };

            var pageIndex = Convert.ToInt32(context.Request.QueryString["pageIndex"]);
            var data = plugin.GetData(context, pageIndex);
            return data;
        }
    }
}