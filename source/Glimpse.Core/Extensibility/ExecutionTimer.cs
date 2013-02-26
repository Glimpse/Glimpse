using System;
using System.Diagnostics;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of <see cref="IExecutionTimer"/>.
    /// </summary>
    public class ExecutionTimer : IExecutionTimer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionTimer" /> class.
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        public ExecutionTimer(Stopwatch stopwatch)
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            Stopwatch = stopwatch;
            RequestStart = DateTime.Now;
        }

        /// <summary>
        /// Gets the request start date and time.
        /// </summary>
        /// <value>
        /// The request start.
        /// </value>
        public DateTime RequestStart { get; private set; }

        /// <summary>
        /// Gets or sets the stopwatch.
        /// </summary>
        /// <value>
        /// The stopwatch.
        /// </value>
        internal Stopwatch Stopwatch { get; set; }

        /// <summary>
        /// Returns a special <see cref="TimerResult "/> which represents a single point in time.
        /// </summary>
        /// <returns>
        /// A TimerResult of a single point in time.
        /// </returns>
        public TimerResult Point()
        {
            return new TimerResult
                {
                    StartTime = DateTime.Now,
                    Offset = Stopwatch.Elapsed,
                    Duration = TimeSpan.Zero
                };
        }

        /// <summary>
        /// Times the specified function. This will start and stop the timer either side
        /// of the function that is executed.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected</typeparam>
        /// <param name="function">The function that should be executed.</param>
        /// <returns>
        /// Timing info that is collected as a result of the execution.
        /// </returns>
        public TimerResult<T> Time<T>(Func<T> function)
        {
            var result = new TimerResult<T>
                {
                    StartTime = DateTime.Now, 
                    Offset = Stopwatch.Elapsed, 
                    Result = function()
                };
            result.Duration = Stopwatch.Elapsed - result.Offset;

            return result;
        }

        /// <summary>
        /// Times the specified action. This will start and stop the timer either side
        /// of the action that is executed.
        /// </summary>
        /// <param name="action">The action that should be executed.</param>
        /// <returns>
        /// Timing info that is collected as a result of the execution.
        /// </returns>
        public TimerResult Time(Action action)
        {
            var result = new TimerResult
                {
                    StartTime = DateTime.Now, 
                    Offset = Stopwatch.Elapsed
                };
            action();
            result.Duration = Stopwatch.Elapsed - result.Offset;
            
            return result;
        }

        /// <summary>
        /// Starts the timer instance.
        /// </summary>
        /// <returns>
        /// TimeSpan that represents this timing instance.
        /// </returns>
        /// <remarks>
        /// If this method is used, it is up to the developer to manually stop the
        /// timer by calling Stop().
        /// </remarks>
        public TimeSpan Start()
        {
            return Stopwatch.Elapsed;
        }

        /// <summary>
        /// Stops the timer instance.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>
        /// Timing info that is collected as a result of the execution.
        /// </returns>
        /// <remarks>
        /// Inconsistent results may be experienced if you do not call Start() before
        /// calling Stop().
        /// </remarks>
        public TimerResult Stop(TimeSpan offset)
        {
            var result = new TimerResult
                {
                    StartTime = DateTime.Now - offset, 
                    Offset = offset
                };
            result.Duration = Stopwatch.Elapsed - result.Offset;

            return result;
        }
    }
}