using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for an execution timer which will track how long given executions take.
    /// </summary>
    public interface IExecutionTimer
    {
        /// <summary>
        /// Gets the request start date and time.
        /// </summary>
        /// <value>
        /// The request start.
        /// </value>
        DateTime RequestStart { get; }

        /// <summary>
        /// Points this instance.
        /// </summary>
        /// <returns>A <see cref="TimerResult"/> which represents a single point in time.</returns>
        TimerResult Point();

        /// <summary>
        /// Times the specified function. This will start and stop the timer either side 
        /// of the function that is executed.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected</typeparam>
        /// <param name="function">The function that should be executed.</param>
        /// <returns>Timing info that is collected as a result of the execution.</returns>
        TimerResult<T> Time<T>(Func<T> function);

        /// <summary>
        /// Times the specified action. This will start and stop the timer either side 
        /// of the action that is executed.
        /// </summary>
        /// <param name="action">The action that should be executed.</param>
        /// <returns>Timing info that is collected as a result of the execution.</returns>
        TimerResult Time(Action action);

        /// <summary>
        /// Starts the timer instance.
        /// </summary>
        /// <returns>TimeSpan that represents this timing instance.</returns>
        /// <remarks>
        /// If this method is used, it is up to the developer to manually stop the 
        /// timer by calling Stop().
        /// </remarks>
        TimeSpan Start();

        /// <summary>
        /// Stops the timer instance.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>Timing info that is collected as a result of the execution.</returns>
        /// <remarks>
        /// Inconsistent results may be experienced if you do not call Start() before
        /// calling Stop().
        /// </remarks>
        TimerResult Stop(TimeSpan offset);
    }
}