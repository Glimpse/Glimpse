using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Mvc3.Plumbing;
using Glimpse.Net;
using Glimpse.WebForms.Extensibility;

namespace Glimpse.Mvc3.Plugin
{
    [GlimpsePlugin(ShouldSetupInInit = true)]
    internal class Execution : IGlimpsePlugin
    {
        public string Name
        {
            get { return "Execution"; }
        }

        public object GetData(HttpApplication application)
        {
            var store = application.Context.Items;
            var calledFiltersMetadata = store[GlimpseConstants.CalledFilters] as List<GlimpseFilterCalledMetadata>;
            var allFiltersMetadata = store[GlimpseConstants.AllFilters] as IList<GlimpseFilterCallMetadata>;

            if (calledFiltersMetadata == null || allFiltersMetadata == null) return null;

            var calledFilterMethods =
                calledFiltersMetadata.Select(
                    called => allFiltersMetadata.Where(filterMetadata => filterMetadata.Guid == called.Guid).Single());
            var unCalledFilterMethods =
                allFiltersMetadata.Where(
                    filterMetadata =>
                    !calledFiltersMetadata.Select(calledFilter => calledFilter.Guid).Contains(filterMetadata.Guid));

            var executed = new List<object[]>
                               {
                                   new[]
                                       {
                                           "Ordinal", "Child", "Category", "Type", "Method", "Time Elapsed", "Order",
                                           "Scope", "Details"
                                       }
                               };

            var count = 0;
            foreach (var metadata in calledFilterMethods)
            {
                var timespan =
                    calledFiltersMetadata.Where(cfm => cfm.Guid == metadata.Guid).Select(cfm => cfm.ExecutionTime).
                        Single();

                var milliseconds = Math.Round(timespan.TotalMilliseconds);
                var timespanDisplay = milliseconds == 0 ? "~ 0 ms" : milliseconds + " ms";


                if (metadata.InnerFilter == null)
                {
                    executed.Add(new object[]
                                     {
                                         count++, metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name,
                                         metadata.Method, timespanDisplay, metadata.Order, metadata.Scope.ToString(), null,
                                         "selected"
                                     });
                }
                else
                {
                    var instance = metadata.InnerFilter.Instance;
                    executed.Add(new[]
                                     {
                                         count++, metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name,
                                         metadata.Method, timespanDisplay, metadata.Order, metadata.Scope.ToString(),
                                         instance is OutputCacheAttribute || instance is HandleErrorAttribute
                                             ? instance
                                             : null
                                     });
                }
            }

            var unexecuted = new List<object[]>
                                 {
                                     new[] {"Child", "Category", "Type", "Method", "Order", "Scope", "Details"}
                                 };

            foreach (var metadata in unCalledFilterMethods)
            {
                var instance = metadata.InnerFilter.Instance;
                unexecuted.Add(new[]
                                   {
                                       metadata.IsChild.ToString(), metadata.Category, metadata.Type.Name, metadata.Method,
                                       metadata.Order, metadata.Scope.ToString(),
                                       instance is OutputCacheAttribute || instance is HandleErrorAttribute
                                           ? instance
                                           : null, "quiet"
                                   });
            }

            if (executed.Count > 1 || unexecuted.Count > 1)
                return new
                           {
                               ExecutedMethods = executed,
                               UnExecutedMethods = unexecuted
                           };

            return null;
        }

        public void SetupInit(HttpApplication application)
        {
            GlimpsePipelineInitiation.ControllerFactory();

            GlimpsePipelineInitiation.DependencyResolver();
        }
    }
}