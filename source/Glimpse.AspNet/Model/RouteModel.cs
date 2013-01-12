using System;
using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    public class RouteModel
    {
        public bool IsMatch { get; set; }

        public string Area { get; set; }

        public string Url { get; set; }

        public IEnumerable<RouteDataItemModel> RouteData { get; set; }

        public IEnumerable<RouteConstraintModel> Constraints { get; set; }

        public IDictionary<string, object> DataTokens { get; set; } 

        public TimeSpan Duration { get; set; }
    }
}
