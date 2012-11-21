using System;

namespace Glimpse.AspNet.Model
{
    public class EnvironmentProcessModel
    {
        public string WorkerProcess { get; set; }

        public int ProcessId { get; set; }

        public DateTime StartTime { get; set; }
    }
}