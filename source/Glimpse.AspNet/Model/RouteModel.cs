using System.Collections.Generic;
using System.Web.Routing;

namespace Glimpse.AspNet.Model
{
    public class RouteModel
    {
        public RouteModel(
            string area,
            string url, 
            IEnumerable<RouteDataItemModel> routeData,
            IEnumerable<RouteConstraintModel> constraints, 
            RouteValueDictionary datatokens)
        {
            Area = area;
            URL = url;
            RouteData = routeData;
            Constraints = constraints;
            DataTokens = datatokens;
        }

        public RouteModel()
        {
        }

        public bool MatchesCurrentRequest { get; set; }
        public string Area { get; set; }
        public string URL { get; set; }
        public IEnumerable<RouteDataItemModel> RouteData { get; set; }
        public IEnumerable<RouteConstraintModel> Constraints { get; set; }
        public RouteValueDictionary DataTokens { get; set; }
        public bool IsFirstMatch { get; set; }
    }

    public class RouteDataItemModel
    {
        public RouteDataItemModel(string key, object defaultValue)
        {
            PlaceHolder = key;
            DefaultValue = defaultValue;
        }

        public string PlaceHolder { get; set; }
        public object DefaultValue { get; set; }
        public object ActualValue { get; set; }
    }

    public class RouteConstraintModel
    {
        public RouteConstraintModel(string parameterName, string constraint, bool @checked, bool matched)
        {
            ParameterName = parameterName;
            Constraint = constraint;
            Checked = @checked;
            Matched = matched;
        }

        /// <summary>
        /// The name of the URL parameter on which this constraint operates
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// String representation of the constraint
        /// </summary>
        public string Constraint { get; set; }

        /// <summary>
        /// True if this constraint was checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// True if this constraint matched
        /// </summary>
        public bool Matched { get; set; }
    }
}
