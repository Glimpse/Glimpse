using System;
using System.Diagnostics;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Extensibility
{
    public class ExecutionTimer : IExecutionTimer
    {
        public ExecutionTimer(Stopwatch stopwatch)
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            Stopwatch = stopwatch;
        }

        internal Stopwatch Stopwatch { get; set; }

        public TimerResult Point()
        {
            var result = new TimerResult();
            result.StartTime = DateTime.Now;
            result.Offset = Stopwatch.Elapsed;
            result.Duration = TimeSpan.Zero;

            return result;
        }

        public TimerResult<T> Time<T>(Func<T> func)
        {
            var result = new TimerResult<T>();
            result.StartTime = DateTime.Now;
            result.Offset = Stopwatch.Elapsed;
            result.Result = func();
            result.Duration = Stopwatch.Elapsed - result.Offset;

            return result;
        }

        public TimerResult Time(Action action)
        {
            var result = new TimerResult();
            result.StartTime = DateTime.Now;
            result.Offset = Stopwatch.Elapsed;
            action();
            result.Duration = Stopwatch.Elapsed - result.Offset;
            
            return result;
        }

        public TimeSpan Start()
        {
            return Stopwatch.Elapsed;
        }

        public TimerResult Stop(TimeSpan offset)
        {
            var result = new TimerResult();
            result.StartTime = DateTime.Now - offset;
            result.Offset = offset;
            result.Duration = Stopwatch.Elapsed - result.Offset;

            return result;
        }
    }
}