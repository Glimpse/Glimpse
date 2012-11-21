using System;

namespace Glimpse.Core.Extensibility
{
    public interface IExecutionTimer
    {
        TimerResult Point();

        TimerResult<T> Time<T>(Func<T> func);
        
        TimerResult Time(Action action);

        double Start();

        TimerResult Stop(double offset);
    }
}