using System;
using System.Reflection;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// The message used to to track the beginning and end of Http requests.
    /// </summary>
    internal class RuntimeMessage : ITimelineMessage, ISourceMessage
    {
        /// <summary>
        /// Gets the id of the request.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the event category.
        /// </summary>
        /// <value>
        /// The event category.
        /// </value>
        public TimelineCategoryItem EventCategory { get; set; }

        /// <summary>
        /// Gets or sets the event sub text.
        /// </summary>
        /// <value>
        /// The event sub text.
        /// </value>
        public string EventSubText { get; set; }

        /// <summary>
        /// Gets or sets the type of the executed.
        /// </summary>
        /// <value>
        /// The type of the executed.
        /// </value>
        public Type ExecutedType { get; set; }

        /// <summary>
        /// Gets or sets the executed method.
        /// </summary>
        /// <value>
        /// The executed method.
        /// </value>
        public MethodInfo ExecutedMethod { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public TimeSpan Offset { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }
    }
}
