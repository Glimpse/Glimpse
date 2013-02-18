namespace Glimpse.Core.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class TimelineMessage
    {
        private static TimelineCategory request = new TimelineCategoryRequest();
        private static TimelineCategory other = new TimelineCategoryOther();

        /// <summary>
        /// Gets the timeline category for a request.
        /// </summary>
        public static TimelineCategory Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the timeline category for a other events.
        /// </summary>
        public static TimelineCategory Other
        {
            get { return other; }
        }
    }
}
