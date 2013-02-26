using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class Timeline : TimelineMessage
    {
        private static TimelineCategory controller = new TimelineCategory("Controller", "#FDBF45", "#DDA431");
        private static TimelineCategory filter = new TimelineCategory("Filter", "#72A3E4", "#5087CF");
        private static TimelineCategory view = new TimelineCategory("View", "#10E309", "#0EC41D");

        /// <summary>
        /// Gets the timeline category for a controller.
        /// </summary>
        public static TimelineCategory Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Gets the timeline category for a filter.
        /// </summary>
        public static TimelineCategory Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Gets a timeline for a view.
        /// </summary>
        public static TimelineCategory View
        {
            get { return view; }
        }
    }
}
