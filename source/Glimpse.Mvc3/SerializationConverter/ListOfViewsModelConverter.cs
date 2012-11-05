using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Model;

namespace Glimpse.Mvc.SerializationConverter
{
    public class ListOfViewsModelConverter : SerializationConverter<List<ViewsModel>>
    {
        public override object Convert(List<ViewsModel> models)
        {
            var result = new List<IEnumerable<object>>
                {
                    new[] { "Ordinal", "Source Controller", "Requested View", "Master Override", "Partial", "View Engine", "Check Cache", "Found", "Details" },
                };

            var count = 0;
            
            // all rows start as selected, but the jagged selected "column" is dropped via Take(8)
            result.AddRange(from item in models
                            let row = new[]
                                          {
                                              count++, // Ordinal
                                              item.SourceController, // Source Controller
                                              item.ViewName, // Requested View
                                              item.MasterName, // Master Override
                                              item.IsPartial, // Partial
                                              item.ViewEngineType, // View Engine
                                              item.UseCache, // Check Cache
                                              item.IsFound, // Found
                                              GetDetails(item), // Details
                                              "selected"
                                          }
                            select item.IsFound ? row : row.Take(row.Length - 1));

            return result;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private object GetDetails(ViewsModel model)
        {
            if (!model.IsFound)
            {
                var searchedLocations = new List<IEnumerable<string>> { new[] { "Not Found In" } };

                if (model.UseCache)
                {
                    // TODO: Wrap "markup" in util library/extensions: string.Underline() or Markdown.Underline(string)
                    searchedLocations.Add(new[] { "_" + model.ViewEngineType.Name + " cache_" });
                }
                else
                {
                    // .ToArray() required for NET35 support
                    searchedLocations.AddRange(model.SearchedLocations.Select(location => new[] { location }).ToArray());
                }

                return searchedLocations;
            }
            
            var summary = model.ViewModelSummary;
            return new Dictionary<string, object>
                       {
                           { "Model Type", summary.ModelType },
                           { "Model State Valid", summary.IsValid },
                           { "TempData Keys", summary.TempDataKeys },
                           { "ViewData Keys", summary.ViewDataKeys },
                       };
        }
    }
}