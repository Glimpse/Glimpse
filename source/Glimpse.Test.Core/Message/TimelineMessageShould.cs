using System;
using System.Collections.Generic;
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
        public void Construct(TimerResult timerResult, string eventName, string eventCategory)
        {
            var message = new TimelineMessage(timerResult, eventName, eventCategory);

            Assert.Equal(timerResult.Duration, message.Duration);
            Assert.Equal(timerResult.Offset, message.Offset);
            Assert.Equal(timerResult.StartTime, message.StartTime);
            Assert.Equal(eventName, message.EventName);
            Assert.Equal(eventCategory, message.EventCategory);
        }

        [Theory, AutoMock]
        public void BuildDetails(TimerResult timerResult, string eventName, string eventCategory)
        {
            var message = new TimelineMessage(timerResult, eventName, eventCategory);

            var dictionary = new Dictionary<string, object>();
            message.BuildDetails(dictionary);

            Assert.Equal(0, dictionary.Count);
        }
    }
}
