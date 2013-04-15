using System;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// Message definition that captures trace events.
    /// </summary>
    public interface ITraceMessage
    {
        /// <summary>
        /// Gets or sets the category the message is in.
        /// </summary>
        /// <value>The category.</value>
        string Category { get; }

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; }

        /// <summary>
        /// Gets or sets the time from the request start.
        /// </summary>
        /// <value>From first.</value>
        TimeSpan FromFirst { get; }

        /// <summary>
        /// Gets or sets the time from the last trace event.
        /// </summary>
        /// <value>From last.</value>
        TimeSpan FromLast { get; }

        /// <summary>
        /// Gets or sets the indent level of the message.
        /// </summary>
        /// <value>The indent level.</value>
        int IndentLevel { get; }
    }
}