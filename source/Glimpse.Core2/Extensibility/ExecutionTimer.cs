using System;
using System.Diagnostics;

namespace Glimpse.Core2.Extensibility
{
    public interface IExecutionTimer
    {
        MethodTimerResult<T> Time<T>(Func<T> func);
        FunctionTimerResult Time(Action action);
    }

    public class ExecutionTimer : IExecutionTimer
    {
        internal Stopwatch Stopwatch { get; set; }

        public ExecutionTimer(Stopwatch stopwatch)
        {
            if (!stopwatch.IsRunning)
                stopwatch.Start();

            Stopwatch = stopwatch;
        }

        public MethodTimerResult<T> Time<T>(Func<T> func)
        {
            var offset = Stopwatch.ElapsedMilliseconds;
            return new MethodTimerResult<T>
                       {
                           Offset = offset,
                           Result = func(),
                           Duration = TimeSpan.FromMilliseconds(Stopwatch.ElapsedMilliseconds - offset)
                       };
        }

        public FunctionTimerResult Time(Action action)
        {
            var offset = Stopwatch.ElapsedMilliseconds;

            action();
            
            return new FunctionTimerResult
                       {
                           Offset = offset,
                           Duration = TimeSpan.FromMilliseconds(Stopwatch.ElapsedMilliseconds - offset)
                       };
        }
    }
}