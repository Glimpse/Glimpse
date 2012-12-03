using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plugin.Assist;

namespace Glimpse.AspNet.SerializationConverter
{
    public class ListOfRouteModelConverter : SerializationConverter<List<RouteModel>>
    {
        public override object Convert(List<RouteModel> routes)
        {
            var section = new TabSection("Area", "Url", "Data", "Constraints", "DataTokens", "Duration");
            foreach (var item in routes)
            {
                section.AddRow().Column(item.Area).Column(item.Url).Column(GetRouteData(item.RouteData)).Column(GetConstraintData(item.Constraints)).Column(GetDataTokens(item.DataTokens)).Column(Math.Round(item.Duration, 2)).SelectedIf(item.IsMatch);
            }

            return section.Build();
        }

        private static object GetDataTokens(IDictionary<string, object> dataTokens)
        {
            if (dataTokens == null)
            {
                return null;
            }

            var section = new TabSection("Data", "Value");
            foreach (var item in dataTokens)
            { 
                section.AddRow().Column(item.Key).Column(item.Value);
            }

            return section;
        }

        private static object GetRouteData(IEnumerable<RouteDataItemModel> routeData)
        {
            if (routeData == null)
            {
                return null;
            }

            var section = new TabSection("Placeholder", "Default", "Actual");
            foreach (var item in routeData)
            {
                var value = item.DefaultValue ?? "MISSING";
                if (value == string.Empty)
                {
                    value = "\"\"";
                }

                section.AddRow().Column(item.PlaceHolder).Column(value).Column(item.ActualValue);
            }

            return section;
        }

        private static object GetConstraintData(IEnumerable<RouteConstraintModel> routeData)
        {
            if (routeData == null)
            {
                return null;
            }

            var section = new TabSection("Parameter Name", "Constraint", "Is Match");
            foreach (var item in routeData)
            {
                section.AddRow().Column(item.ParameterName).Column(item.Constraint).Column(item.IsMatch);
            }

            return section;
        } 
    }
}
