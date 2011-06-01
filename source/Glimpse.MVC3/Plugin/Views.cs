using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc3.Plumbing;

namespace Glimpse.Mvc3.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Views : IGlimpsePlugin, IProvideGlimpseHelp
    {
        public string Name
        {
            get { return "Views"; }
        }

        public object GetData(HttpContextBase context)
        {
            var store = context.Items;
            var data = store[GlimpseConstants.ViewEngine] as IList<GlimpseViewEngineCallMetadata>;
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
                        locations.AddRange(
                            callMetadata.ViewEngineResult.SearchedLocations.Select(location => new object[] {location}));

                    result.Add(new object[]
                                   {
                                       result.Count, callMetadata.ViewName, callMetadata.MasterName,
                                       callMetadata.IsPartial.ToString(), viewEngineName,
                                       callMetadata.UseCache.ToString(), isFound.ToString(), locations
                                   });
                }
                else
                {
                    object model = null;

                    if (callMetadata.GlimpseView != null)
                    {
                        var viewContext = callMetadata.GlimpseView.ViewContext;
                        object modelResult = null;
                        var vd = viewContext.ViewData.Flatten();
                        var td = viewContext.TempData.Flatten();
                        var vm = viewContext.ViewData.Model;

                        if (vm != null) modelResult = new {ModelType = vm.GetType().ToString(), Value = vm};

                        model = new
                                    {
                                        ViewData = vd.Count > 0 ? vd : null,
                                        Model = vm != null ? modelResult : null,
                                        TempData = td.Count > 0 ? td : null,
                                    };
                    }

                    result.Add(new[]
                                   {
                                       result.Count, callMetadata.ViewName, callMetadata.MasterName,
                                       callMetadata.IsPartial.ToString(), viewEngineName,
                                       callMetadata.UseCache.ToString(), isFound.ToString(), model, "selected"
                                   });
                }
            }

            return result;
        }

        public void SetupInit()
        {
            var engines = ViewEngines.Engines;

            for (var i = 0; i < engines.Count; i++)
            {
                if (!(engines[i] is GlimpseViewEngine))
                    engines[i] = new GlimpseViewEngine(engines[i]);
            }
        }

        public string HelpUrl
        {
            get { return "http://getGlimpse.com/Help/Plugin/Views"; }
        }
    }
}