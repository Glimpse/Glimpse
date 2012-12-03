using System.Collections.Generic;
using System.Web.Routing;

namespace Glimpse.AspNet.Model
{
    public class RouteModel
    {
        public bool MatchesCurrentRequest { get; set; }

        public string Area { get; set; }

        public string Url { get; set; }

        public IEnumerable<RouteDataItemModel> RouteData { get; set; }

        public IEnumerable<RouteConstraintModel> Constraints { get; set; }

        public IDictionary<string, object> DataTokens { get; set; }

        public bool IsFirstMatch { get; set; }

        public double Duration { get; set; }
    }
}
