using Glimpse.Core.Message;

namespace Glimpse.Mvc.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class Timeline : Core.Message.TimelineMessage
    {
        private static TimelineCategory controller = new TimelineCategoryController();
        private static TimelineCategory filter = new TimelineCategoryFilter();
        private static TimelineCategory view = new TimelineCategoryView();

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
