using TimelineCategoryItem = Glimpse.Core.Message.TimelineCategoryItem;

namespace Glimpse.WebForms.Message
{
    /// <summary>
    /// Options that can used for timeline events.
    /// </summary>
    public class WebFormsTimelineCategory : Glimpse.Core.Message.TimelineCategory
    {
        private static TimelineCategoryItem webForms = new TimelineCategoryItem("WebForms", "#FDBF45", "#DDA431"); 

        /// <summary>
        /// Gets the timeline category for a webForms.
        /// </summary>
        public static TimelineCategoryItem WebForms
        {
            get { return webForms; }
        }
    }
}
