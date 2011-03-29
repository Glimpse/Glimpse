using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class ViewData : DictionaryStringObjectPlugin
    {
        public override void SetupInit()
        {
            var filters = GlobalFilters.Filters;

            if (!filters.OfType<GlimpseFilterAttribute>().Any())
                filters.Add(new GlimpseFilterAttribute(), int.MinValue);
        }

        public override string Name
        {
            get { return "View Data"; }
        }

        public override object GetData(HttpApplication application)
        {
            var store = application.Context.Items;

            var data = store[GlimpseConstants.ViewData] as ViewDataDictionary;
            if (data == null) return null;

            var tempData = store[GlimpseConstants.TempData] as TempDataDictionary;

            return new
                       {
                           ViewData = Process(data, application),
                           data.Model,
                           TempData = Process(tempData, application)
        };
        }
    }
}
