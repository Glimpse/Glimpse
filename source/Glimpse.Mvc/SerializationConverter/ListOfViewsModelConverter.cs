using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    using Core.Tab.Assist;

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

            var section = new TabObject();
            var summary = model.ViewModelSummary;

            if (summary == null)
            {
                return section;
            }

            if (summary.HasDisplayMode)
            {
                section.AddRow().Key("Display Mode").Value(summary.DisplayModeName);                
            }

            section.AddRow().Key("Model Type").Value(summary.ModelType);
            section.AddRow().Key("Model State Valid").Value(summary.IsValid);
            section.AddRow().Key("TempData Keys").Value(summary.TempDataKeys);
            section.AddRow().Key("ViewData Keys").Value(summary.ViewDataKeys);

            return section;
        }
    }
}