using System; 
using System.Reflection; 

namespace Glimpse.Mvc.Message
{
    public interface IExecutionMessage
    {
        bool IsChildAction { get; }

        Type ExecutedType { get; }

        MethodInfo ExecutedMethod { get; }

        TimeSpan Duration { get; } 
    }
}
