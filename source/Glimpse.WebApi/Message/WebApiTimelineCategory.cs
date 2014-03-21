using TimelineCategoryItem = Glimpse.Core.Message.TimelineCategoryItem;

namespace Glimpse.WebApi.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class WebApiMvcTimelineCategory : Glimpse.Core.Message.TimelineCategory
    {
        private static TimelineCategoryItem controller = new TimelineCategoryItem("Controller", "#FDBF45", "#DDA431");
        private static TimelineCategoryItem filter = new TimelineCategoryItem("Filter", "#72A3E4", "#5087CF");
        private static TimelineCategoryItem value = new TimelineCategoryItem("Value", "#10E309", "#0EC41D");

        /// <summary>
        /// Gets the timeline category for a controller.
        /// </summary>
        public static TimelineCategoryItem Controller
        {
            get { return controller; }
        }

        /// <summary>
        /// Gets the timeline category for a filter.
        /// </summary>
        public static TimelineCategoryItem Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Gets a timeline for a view.
        /// </summary>
        public static TimelineCategoryItem Value
        {
            get { return value; }
        }
    }
}
