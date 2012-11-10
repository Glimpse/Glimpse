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

        public TimerResult<T> Time<T>(Func<T> func)
        {
            var result = new TimerResult<T>();
            result.StartTime = DateTime.Now;
            result.Offset = Stopwatch.ElapsedTicks.ConvertNanosecondsToMilliseconds();
            result.Result = func();
            result.Duration = Stopwatch.ElapsedTicks.ConvertNanosecondsToMilliseconds() - result.Offset;

            return result;
        }

        public TimerResult Time(Action action)
        {
            var result = new TimerResult();
            result.StartTime = DateTime.Now;
            result.Offset = Stopwatch.ElapsedTicks.ConvertNanosecondsToMilliseconds();
            action();
            result.Duration = Stopwatch.ElapsedTicks.ConvertNanosecondsToMilliseconds() - result.Offset;
            
            return result;
        }

        public double Start()
        {
            return Stopwatch.ElapsedMilliseconds.ConvertNanosecondsToMilliseconds();
        }

        public TimerResult Stop(double offset)
        {
            var result = new TimerResult();
            result.StartTime = DateTime.Now.AddMilliseconds(offset * -1);
            result.Offset = offset;
            result.Duration = Stopwatch.ElapsedTicks.ConvertNanosecondsToMilliseconds() - result.Offset;

            return result;
        }
    }
}