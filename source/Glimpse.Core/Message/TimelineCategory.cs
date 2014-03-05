namespace Glimpse.Core.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class TimelineCategory
    {
        private static TimelineCategoryItem request = new TimelineCategoryItem("Common", "#AF78DD", "#823BBE");
        private static TimelineCategoryItem other = new TimelineCategoryItem("Other", "#EEEEEE", "#CCCCCC");
        private static TimelineCategoryItem user = new TimelineCategoryItem("User", "#3c454f", "#eee");

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

        /// <summary>
        /// Gets the timeline category for a user event.
        /// </summary>
        public static TimelineCategoryItem User
        {
            get { return user; }
        }
    }
}
