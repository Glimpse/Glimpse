using System;

namespace Glimpse.Core.Extensibility
{
    public class TimerResult
    {
        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }
    }
}