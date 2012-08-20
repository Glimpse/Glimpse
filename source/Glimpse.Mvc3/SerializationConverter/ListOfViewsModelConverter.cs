using System.Linq;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.Model;

namespace Glimpse.Mvc3.SerializationConverter
{
    public class ListOfViewsModelConverter:SerializationConverter<List<ViewsModel>>
    {
        public override object Convert(List<ViewsModel> models)
        {
            var result = new List<IEnumerable<object>>
                {
                    new []{"Ordinal", "Requested View", "Master Override", "Partial", "View Engine", "Check Cache", "Found", "Details"},
                };

            var count = 0;
            result.AddRange(from item in models
                            let row = new []
                                          {
                                              count++, //Ordinal
                                              item.ViewName, //Requested View
                                              item.MasterName, //Master Override
                                              item.IsPartial, //Partial
                                              item.ViewEngineType, //View Engine
                                              item.UseCache, //Check Cache
                                              item.IsFound, //Found
                                              GetDetails(item), //Details
                                              "selected"
                                          }
                            select item.IsFound ? row : row.Take(row.Length-1)); //all rows start as selected, but the jagged selected "column" is dropped via Take(8)

            return result;
        }

        private object GetDetails(ViewsModel model)
        {
            if (!model.IsFound)
            {
                var searchedLocations = new List<IEnumerable<string>> {new[] {"Not Found In"}};

                if (model.UseCache)
                    searchedLocations.Add(new[] {"_" + model.ViewEngineType.Name + " cache_"});//TODO: Wrap "markup" in util library/extensions: string.Underline() or Markdown.Underline(string)
                else
                    searchedLocations.AddRange(model.SearchedLocations.Select(location => new[] {location}));

                return searchedLocations;
            }
            else
            {
                var vmSummary = model.ViewModelSummary;
                return new Dictionary<string, object>
                           {
                               {"Model Type", vmSummary.ModelType},
                               {"Model State Valid", vmSummary.IsValid},
                               {"TempData Keys", vmSummary.TempDataKeys},
                               {"ViewData Keys", vmSummary.ViewDataKeys},
                           };
            }
        }
    }
}