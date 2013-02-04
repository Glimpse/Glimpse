using System;
using System.Collections.Generic;
using System.Reflection;

using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Message
{
    public class TimelineMessageShould
    {
        [Theory, AutoMock]
        public void Construct(TimerResult timerResult, Type type, MethodInfo methodInfo, string eventName, TimelineCategory eventCategory)
        {
            var message = new TimelineMessage(timerResult, type, methodInfo, eventName, eventCategory);

            Assert.Equal(timerResult.Duration, message.Duration);
            Assert.Equal(timerResult.Offset, message.Offset);
            Assert.Equal(timerResult.StartTime, message.StartTime);
            Assert.Equal(eventName, message.EventName);
            Assert.Equal(eventCategory, message.EventCategory);
            Assert.Equal(type, message.ExecutedType);
            Assert.Equal(methodInfo, message.ExecutedMethod);
        }

        [Theory, AutoMock]
        public void BuildDetails(TimerResult timerResult, Type type, MethodInfo methodInfo, string eventName, TimelineCategory eventCategory)
        {
            var message = new TimelineMessage(timerResult, type, methodInfo, eventName, eventCategory);

            var dictionary = new Dictionary<string, object>();
            message.BuildDetails(dictionary);

            Assert.Equal(0, dictionary.Count);
        }
    }
}
