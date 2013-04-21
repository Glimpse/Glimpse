using System;
using System.Collections.Generic;

namespace Glimpse.Core.Model
{
    /// <summary>
    /// Model that represents an individual event that is shown in 
    /// the timeline.
    /// </summary>
    public class TimelineEventModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineEventModel"/> class.
        /// </summary> 
        public TimelineEventModel()
        {
            Details = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the category is attached to.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the start point in the total request timespan where the event starts.
        /// </summary>
        /// <value>The start point.</value>
        public TimeSpan StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the duration of the event.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the title used to describe the event.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the sub text used to describe the event.
        /// </summary>
        /// <value>The sub text.</value>
        public string SubText { get; set; }

        /// <summary>
        /// Gets or sets the details used to describe the event.
        /// </summary>
        /// <value>The details.</value>
        public IDictionary<string, object> Details { get; set; }

        /// <summary>
        /// Gets the end point in the total request timespan where the event finishes.
        /// </summary>
        /// <value>The end point.</value>
        public TimeSpan EndPoint
        {
            get { return StartPoint + Duration; }
        } 
    }
}