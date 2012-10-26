using System;

namespace Glimpse.Core.Extensibility
{
    public interface IExecutionTimer
    {
        TimerResult<T> Time<T>(Func<T> func);
        
        TimerResult Time(Action action);
        
        long Start();
        
        TimerResult Stop(long offset);
    }
}