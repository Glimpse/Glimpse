using System;
using System.Collections.Generic;

namespace Glimpse.Core.Model
{
    /// <summary>
    /// Model that is used to populate the timeline
    /// </summary>
    public class TimelineModel
    {
        /// <summary>
        /// Gets or sets the overall duration of the request.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the categories which are available.
        /// </summary>
        /// <value>The category.</value>
        public IDictionary<string, TimelineCategoryModel> Category { get; set; }

        /// <summary>
        /// Gets or sets the events that make up the request.
        /// </summary>
        /// <value>The events.</value>
        public IList<TimelineEventModel> Events { get; set; }
    }
}
