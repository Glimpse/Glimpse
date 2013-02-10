using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The result of a <c>Time</c> method call on a <see cref="IExecutionTimer"/>.
    /// </summary>
    public class TimerResult
    {
        /// <summary>
        /// Gets or sets the offset from the beginning of the Http request.
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