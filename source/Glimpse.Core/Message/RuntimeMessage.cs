using System;
using System.Reflection;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// The message used to to track the beginning and end of Http requests.
    /// </summary>
    internal class RuntimeMessage : MessageBase, ITimelineMessage, ISourceMessage
    {   
        public TimeSpan Offset { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime StartTime { get; set; }

        public string EventName { get; set; }

        public TimelineCategoryItem EventCategory { get; set; }

        public string EventSubText { get; set; }

        public Type ExecutedType { get; set; }

        public MethodInfo ExecutedMethod { get; set; }
    }
}
