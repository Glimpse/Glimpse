using System;

namespace Glimpse.Core.Message
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