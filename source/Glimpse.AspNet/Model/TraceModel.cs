using System;

namespace Glimpse.AspNet.Model
{
    public class TraceModel
    {
        public string Category { get; set; }

        public string Message { get; set; }

        public TimeSpan FromFirst { get; set; }

        public TimeSpan FromLast { get; set; }

        public int IndentLevel { get; set; }
    }
}
