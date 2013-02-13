using System;

namespace Glimpse.AspNet.Message
{
    public interface ITraceMessage
    {
        string Category { get; }

        string Message { get; }
        
        TimeSpan FromFirst { get; }
        
        TimeSpan FromLast { get; }
        
        int IndentLevel { get; }
    }
}