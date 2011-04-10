using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Plumbing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Views : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Views"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var data = store[GlimpseConstants.ViewEngine] as IList<GlimpseViewEngineCallMetadata>;
            var vData = store[GlimpseConstants.ViewData] as ViewDataDictionary;
            var tData = store[GlimpseConstants.TempData] as TempDataDictionary;

            if (data == null) return null;

            var result = new List<object[]>
                             {
                                 new[]
                                     {
                                         "Ordinal", "Requested View", "Master Override", "Partial", "View Engine",
                                         "Check Cache", "Found", "Details"
                                     }
                             };


            foreach (var callMetadata in data)
            {
                bool isFound = true;
                string viewEngineName = callMetadata.ViewEngineName;
                var locations = new List<object[]> {new[] {"Not Found In"}};

                if (callMetadata.ViewEngineResult.View == null)
                {
                    isFound = false;

                    if (callMetadata.UseCache)
                        locations.Add(new object[] {"_" + viewEngineName + " cache_"});
                    else
                        locations.AddRange(callMetadata.ViewEngineResult.SearchedLocations.Select(location => new object[] {location}));

                    result.Add(new object[]
                                    {
                                        result.Count, callMetadata.ViewName, callMetadata.MasterName, callMetadata.IsPartial.ToString(), viewEngineName,
                                        callMetadata.UseCache.ToString(), isFound.ToString(), locations
                                    });
                }
                else
                {
                    object model = null;

                    if (callMetadata.GlimpseView != null)
                    {
                        var vd = callMetadata.GlimpseView.ViewContext.ViewData;
                        vData = vd;
                        tData = callMetadata.GlimpseView.ViewContext.TempData;
                        if (vd.Model != null) model = new {ModelType = vd.Model.GetType().ToString(), Value = vd.Model};
                    }

                    result.Add(new[]
                                    {
                                        result.Count, callMetadata.ViewName, callMetadata.MasterName, callMetadata.IsPartial.ToString(), viewEngineName,
                                        callMetadata.UseCache.ToString(), isFound.ToString(), model, "selected"
                                    });
                }
            }

            return new {RequestedViews = result, ViewData = vData.Flatten(), TempData= tData.Flatten()};
        }

        

        public void SetupInit(HttpApplication application)
        {
            var engines = ViewEngines.Engines;

            for (var i = 0; i < engines.Count; i++)
            {
                if (!(engines[i] is GlimpseViewEngine))
                    engines[i] = new GlimpseViewEngine(engines[i]);
            }

            /*var filters = GlobalFilters.Filters;

            if (!filters.OfType<GlimpseFilterAttribute>().Any())
                filters.Add(new GlimpseFilterAttribute(), int.MinValue);*/
        }
    }
}