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
                section.AddRow().Column(item.Area).Column(item.Url).Column(GetRouteData(item.RouteData)).Column(GetConstraintData(item.Constraints)).Column(item.DataTokens).Column(Math.Round(item.Duration, 2)).SelectedIf(item.IsFirstMatch);
            }

            return section.Build();
        }

        private static object GetRouteData(IEnumerable<RouteDataItemModel> routeData)
        {
            if (routeData == null)
            {
                return null;
            }

            var section = new TabSection("Placeholder", "Default Value", "Actual Value");
            foreach (var item in routeData)
            {
                section.AddRow().Column(item.PlaceHolder).Column(item.DefaultValue).Column(item.ActualValue);
            }

            return section;
        }

        private static object GetConstraintData(IEnumerable<RouteConstraintModel> routeData)
        {
            if (routeData == null)
            {
                return null;
            }

            var section = new TabSection("Parameter Name", "Constraint", "Constraint Checked");
            foreach (var item in routeData)
            {
                section.AddRow().Column(item.ParameterName).Column(item.Constraint).Column(item.Checked).SelectedIf(item.Matched);
            }

            return section;
        } 
    }
}
