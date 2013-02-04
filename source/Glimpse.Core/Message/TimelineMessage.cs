using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Core.Message
{
    public class TimelineMessage : TimeMessage
    {
        public TimelineMessage(TimerResult timerResult, Type executedType, MethodInfo executedMethod, string eventName = null, TimelineCategory eventCategory = null)
            : base(timerResult, executedType, executedMethod)
        {
            EventName = eventName;
            EventCategory = eventCategory;
        }

        public string EventName { get; protected set; }

        public TimelineCategory EventCategory { get; protected set; }

        public string EventSubText { get; protected set; }
        
        public virtual void BuildDetails(IDictionary<string, object> details)
        { 
        }
    }
}