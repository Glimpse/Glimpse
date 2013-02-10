using System; 
using System.Reflection; 

namespace Glimpse.Core.Message
{
    public interface ISourceMessage
    {
        Type ExecutedType { get; set; }

        MethodInfo ExecutedMethod { get; set; }
    }

    public static class SourceMessageExtension
    {
        public static T AsSourceMessage<T>(this T message, Type executedType, MethodInfo executedMethod)
            where T : ISourceMessage
        {
            message.ExecutedType = executedType;
            message.ExecutedMethod = executedMethod; 

            return message;
        }
    }
}
