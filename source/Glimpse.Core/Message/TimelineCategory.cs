namespace Glimpse.Core.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class TimelineCategory
    {
        private static TimelineCategoryItem request = new TimelineCategoryItem("Common", "#AF78DD", "#823BBE");
        private static TimelineCategoryItem other = new TimelineCategoryItem("Other", "#EEEEEE", "#CCCCCC");

        /// <summary>
        /// Gets the timeline category for a request.
        /// </summary>
        public static TimelineCategoryItem Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the timeline category for a other events.
        /// </summary>
        public static TimelineCategoryItem Other
        {
            get { return other; }
        }
    }
}
