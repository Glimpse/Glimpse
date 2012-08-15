using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;
using Glimpse.Core2.Extensibility;


namespace Glimpse.AspNet.SerializationConverter
{
    class ListOfRouteModelConverter : SerializationConverter<List<RouteModel>>
    {
        public override object Convert(List<RouteModel> routes)
        {
            var result = new List<IEnumerable<object>>
                {
                    new []{"Match", "Area", "Url", "Data", "Constraints", "DataTokens"},
                };

            result.AddRange(from item in routes
                            let row = new []
                            {
                                item.MatchesCurrentRequest, // match
                                item.Area,        // area
                                item.URL,         // Url
                                GetRouteData(item.RouteData), // route data
                                item.Constraints, // Constraints
                                item.DataTokens,  // DataTokens
                                "selected"
                            }
                            select item.IsFirstMatch ? row : row.Take(row.Length - 1)); //all rows start as selected, but the jagged selected "column" is dropped via Take(8)

            return result;
        }

        private static object GetRouteData(IEnumerable<RouteDataItemModel> routeData)
        {
            if (routeData == null)
                return null;

            var result = new List<object[]>
                {
                    new object[]{"Placeholder", "Default Value", "Actual Value"},
                };

            var items = from item in routeData
                        select new[]
                                   {
                                       item.PlaceHolder,
                                       item.DefaultValue,
                                       item.ActualValue
                                   };

            result.AddRange(items);

            return result;
        }
    }
}
