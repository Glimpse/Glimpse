using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Glimpse.WebApi.Core.AlternateType
{
    public class IHttpRouteConstraintRegex : System.Web.Http.Routing.IHttpRouteConstraint
    {
        public IHttpRouteConstraintRegex(string constraint)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException("constraint");
            }

            Constraint = constraint;
        }

        public string Constraint { get; set; }

        public bool Match(System.Net.Http.HttpRequestMessage request, System.Web.Http.Routing.IHttpRoute route, string parameterName, System.Collections.Generic.IDictionary<string, object> values, System.Web.Http.Routing.HttpRouteDirection routeDirection)
        {
            object obj;
            values.TryGetValue(parameterName, out obj);
            return Regex.IsMatch(Convert.ToString(obj, (IFormatProvider)CultureInfo.InvariantCulture), "^(" + Constraint + ")$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }
}
