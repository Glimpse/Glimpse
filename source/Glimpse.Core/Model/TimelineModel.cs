using System;
using System.Collections.Generic;

namespace Glimpse.Core.Model
{
    public class TimelineModel
    {
        public TimeSpan Duration { get; set; }

        public IDictionary<string, TimelineCategoryModel> Category { get; set; }

        public IList<TimelineEventModel> Events { get; set; }
    }
}
