using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.AspNet.Model
{
    public class TimelineModel
    {
        public double Duration { get; set; }

        public IDictionary<string, TimelineCategoryModel> Category { get; set; }

        public IList<TimelineEventModel> Events { get; set; }
    }
}
