using System.Web;
using System.Web.Mvc;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin]
    public class ViewData : DictionaryStringObjectPlugin
    {
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
