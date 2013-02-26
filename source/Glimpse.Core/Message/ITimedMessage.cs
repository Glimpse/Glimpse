using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// The definition of a message which is published with execution timing information.
    /// </summary>
    public interface ITimedMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the offset from the Http request start.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        TimeSpan Offset { get; set; }

        /// <summary>
        /// Gets or sets the duration of the executed method.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        DateTime StartTime { get; set; }
    }

    /// <summary>
    /// Extension methods for populating <see cref="ITimedMessage"/> instances.
    /// </summary>
    public static class TimedMessageExtension
    {
        /// <summary>
        /// Populates relevant properties on the source message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="timerResult">The timer result.</param>
        /// <returns>The <paramref name="message"/> with populated <see cref="ITimedMessage"/> properties.</returns>
        public static T AsTimedMessage<T>(this T message, TimerResult timerResult)
            where T : ITimedMessage
        {
            message.Offset = timerResult.Offset;  
            message.Duration = timerResult.Duration; 
            message.StartTime = timerResult.StartTime; 

            return message;
        }
    }
}