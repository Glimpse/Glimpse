using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Extensibility
{
    public interface ITimelineEvent
    {
        string Category { get; set; }

        DateTime StartTime { get; set; }

        double StartPoint { get; set; }

        double Duration { get; set; }

        string Title { get; set; }

        string SubText { get; set; }

        IDictionary<string, object> Details { get; set; }
    }
}
