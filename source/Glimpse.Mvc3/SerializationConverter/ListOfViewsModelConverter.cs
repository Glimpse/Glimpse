using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    using Glimpse.Core.Plugin.Assist;

    public class ListOfViewsModelConverter : SerializationConverter<List<ViewsModel>>
    {
        public override object Convert(List<ViewsModel> models)
        {
            var count = 0;

            var root = new TabSection("Ordinal", "Source Controller", "Requested View", "Master Override", "Partial", "View Engine", "Check Cache", "Found", "Details");
            foreach (var item in models)
            {
                root.AddRow().Column(count++).Column(item.SourceController).Column(item.ViewName).Column(item.MasterName).Column(item.IsPartial).Column(item.ViewEngineType).Column(item.UseCache).Column(item.IsFound).Column(GetDetails(item)).SelectedIf(item.IsFound);
            }
             
            return root.Build();
        }
        
        private object GetDetails(ViewsModel model)
        {
            if (!model.IsFound)
            { 
                if (model.UseCache)
                {
                    return "Not Found In Cache";
                }

                var searchedLocations = new TabSection("Not Found In");
                foreach (var searchedLocation in model.SearchedLocations)
                {
                    searchedLocations.AddRow().Column(searchedLocation);
                }  

                return searchedLocations;
            }

            var summary = model.ViewModelSummary;

            var section = new TabSection("Key", "Value");
            section.AddRow().Column("Model Type").Column(summary.ModelType);
            section.AddRow().Column("Model State Valid").Column(summary.IsValid);
            section.AddRow().Column("TempData Keys").Column(summary.TempDataKeys);
            section.AddRow().Column("ViewData Keys").Column(summary.ViewDataKeys);

            return section;
        }
    }
}