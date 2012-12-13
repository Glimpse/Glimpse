using System; 
using System.Reflection; 

using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Message
{
    public class PointTimelineMessage : TimelineMessage, ITimelineMessage
    {
        public PointTimelineMessage(TimerResult timerResult, Type executedType, MethodInfo executedMethod, string eventName = null, string eventCategory = null)
            : base(timerResult, executedType, executedMethod, eventName, eventCategory)
        {
        }
    }
}
