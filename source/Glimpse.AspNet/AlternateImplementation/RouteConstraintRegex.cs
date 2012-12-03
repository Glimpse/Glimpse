using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Glimpse.AspNet.AlternateImplementation
{
    public class RouteConstraintRegex : IRouteConstraint
    {
        public RouteConstraintRegex(string constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException("constraint");
            }

            Constraint = constraint;
        }

        public string Constraint { get; set; }

        public bool Match(HttpContextBase httpContext, System.Web.Routing.Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object obj;
            values.TryGetValue(parameterName, out obj);
            return Regex.IsMatch(Convert.ToString(obj, (IFormatProvider)CultureInfo.InvariantCulture), "^(" + Constraint + ")$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }
}
