using TimelineCategoryItem = Glimpse.Core.Message.TimelineCategoryItem;

namespace Glimpse.Ado.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class AdoTimelineCategory : Glimpse.Core.Message.TimelineCategory
    {
        private static TimelineCategoryItem connection = new TimelineCategoryItem("Connection", "#F0ED5D", "#DEE81A");
        private static TimelineCategoryItem command = new TimelineCategoryItem("Command", "#FD45F7", "#DD31DA"); 
         
        /// <summary>
        /// Gets the timeline category for a connection.
        /// </summary>
        public static TimelineCategoryItem Connection
        {
            get { return connection; }
        }

        /// <summary>
        /// Gets the timeline category for a command.
        /// </summary>
        public static TimelineCategoryItem Command
        {
            get { return command; }
        }
    }
}
