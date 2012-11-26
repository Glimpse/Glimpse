using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;


namespace Glimpse.AspNet.SerializationConverter
{
    public class ListOfRouteModelConverter : SerializationConverter<List<RouteModel>>
    {
        public override object Convert(List<RouteModel> routes)
        {
            var result = new List<IEnumerable<object>>
                {
                    new[] { "Match", "Area", "Url", "Data", "Constraints", "DataTokens" },
                };

            result.AddRange(from item in routes
                            let row = new[]
                            {
                                item.MatchesCurrentRequest, // match
                                item.Area,        // area
                                item.URL,         // Url
                                GetRouteData(item.RouteData), // route data
                                GetConstraintData(item.Constraints), // Constraints
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
                    new object[] { "Placeholder", "Default Value", "Actual Value" },
                };

            var items = from item in routeData
                        select new[]
                                   {
                                       item.PlaceHolder,
                                       item.DefaultValue,
                                       item.ActualValue
                                   };

            // other plugins can specify custom SerializationConverters for Route data item value types.
            // For instance, Glimpse.Mvc has a SerializationConverter<UrlParameter>, which checks for
            // the special "optional" parameter value and formats it specially.

            result.AddRange(items);

            return result;
        }

        private static object GetConstraintData(IEnumerable<RouteConstraintModel> routeData)
        {
            if (routeData == null)
                return null;

            var result = new List<object[]>
                {
                    new object[] { "Parameter Name", "Constraint", "Constraint Checked", "Constraint Matched" },
                };

            var items = from item in routeData
                        select new object[]
                                   {
                                       item.ParameterName,
                                       item.Constraint,
                                       item.Checked,
                                       item.Checked ? (bool?)item.Matched : null
                                   };
            result.AddRange(items);

            return result;
        }

    }
}
