using System;
using System.Collections.Generic;

namespace Glimpse.AspNet.Model
{
    using Glimpse.Core.Extensibility;

    public class TimelineEventModel
    {
        public TimelineEventModel()
        {
            Details = new Dictionary<string, object>();
        }

        public string Category { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan StartPoint { get; set; }

        public TimeSpan Duration { get; set; }

        public string Title { get; set; }

        public string SubText { get; set; }

        public IDictionary<string, object> Details { get; set; }

        public TimeSpan EndPoint
        {
            get { return StartPoint + Duration; }
        } 
    }
}