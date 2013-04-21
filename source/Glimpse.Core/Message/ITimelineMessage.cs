using System.Collections.Generic;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// Describes the message that the timeline will look for
    /// when recording events.
    /// </summary>
    public interface ITimelineMessage : ITimedMessage
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        string EventName { get; set; }

        /// <summary>
        /// Gets or sets the event category.
        /// </summary>
        /// <value>The event category.</value>
        TimelineCategoryItem EventCategory { get; set; }

        /// <summary>
        /// Gets or sets the event sub text.
        /// </summary>
        /// <value>The event sub text.</value>
        string EventSubText { get; set; }
    }

    /// <summary>
    /// Class TimelineMessageExtension
    /// </summary>
    public static class TimelineMessageExtension
    {
        /// <summary>
        /// Extension method that makes building up a timeline event easy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventCategory">The event category.</param>
        /// <param name="eventSubText">The event sub text.</param>
        /// <returns>Original message instance.</returns>
        public static T AsTimelineMessage<T>(this T message, string eventName, TimelineCategoryItem eventCategory, string eventSubText = null)
            where T : ITimelineMessage
        {
            message.EventName = eventName;
            message.EventCategory = eventCategory;
            message.EventSubText = eventSubText;

            return message;
        }

        /// <summary>
        /// Extension method that makes building up a timeline event easy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="eventCategory">The event category.</param>
        /// <param name="eventSubText">The event sub text.</param>
        /// <returns>Original message instance.</returns>
        public static T AsTimelineMessage<T>(this T message, TimelineCategoryItem eventCategory, string eventSubText = null)
            where T : ITimelineMessage
        { 
            message.AsTimelineMessage(string.Empty, eventCategory, eventSubText);

            return message;
        }
    }
}