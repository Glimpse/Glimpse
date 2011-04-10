using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Net.Plumbing;
using Glimpse.Protocol;

namespace Glimpse.Net.Plugin.Mvc
{
    //TODO: CLEAN ME! Very quickly haccked together!
    [GlimpsePlugin(ShouldSetupInInit = true)]
    public class Execution:IGlimpsePlugin
    {
        public string Name
        {
            get { return "Execution"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var calledFiltersIds = store[GlimpseConstants.CalledFilters] as List<Guid>;
            var allFilters = store[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

            if (calledFiltersIds == null || allFilters == null) return null;

            var calledFilterMethods = calledFiltersIds.Select(guid => allFilters.Where(f => f.Guid == guid).Single());
            var unCalledFilterMethods = allFilters.Where(f=>!calledFiltersIds.Contains(f.Guid));

            var executed = new List<object[]>
                                    {
                                        new []{ "Ordinal", "Child", "Category", "Type", "Method", "Order", "Scope", "Details" }
                                    };

            var count = 0;
            foreach (var metadata in calledFilterMethods)
            {
                if (metadata.InnerFilter == null)
                {
                    executed.Add(new object[] { count++, metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name, metadata.Method, metadata.Order, metadata.Scope.ToString(), null, "selected" });
                }
                else
                {
                    var instance = metadata.InnerFilter.Instance;
                    executed.Add(new[] { count++, metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name, metadata.Method, metadata.Order, metadata.Scope.ToString(), instance is OutputCacheAttribute || instance is HandleErrorAttribute ? instance : null });
                }
            }

            var unexecuted = new List<object[]>
                                    {
                                        new []{ "Child", "Category", "Type", "Method", "Order", "Scope", "Details" }
                                    };

            foreach (var metadata in unCalledFilterMethods)
            {
                var instance = metadata.InnerFilter.Instance;
                unexecuted.Add(new[] { metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name, metadata.Method, metadata.Order, metadata.Scope.ToString(), instance is OutputCacheAttribute || instance is HandleErrorAttribute ? instance : null, "quiet" });
            }

            if (executed.Count == 1) return null;

            return new {
                           ExecutedMethods = executed,
                           UnExecutedMethods = unexecuted
                        };
        }

        public void SetupInit(HttpApplication application)
        {
            //TODO: Wrap up dependency resolver
            var setFactory = ControllerBuilder.Current.GetControllerFactory();
            if (!(setFactory is GlimpseControllerFactory))
                ControllerBuilder.Current.SetControllerFactory(new GlimpseControllerFactory(setFactory));
        }
    }
}
