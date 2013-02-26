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
        public void ShouldBeAbleToBuildWithFactory(TimerResult timerResult, string eventName, TimelineCategory eventCategory, string eventSubText)
        {
            var testMessage = new TestMessage().AsTimelineMessage(eventName, eventCategory, eventSubText);

            Assert.Equal(eventName, testMessage.EventName);
            Assert.Equal(eventCategory, testMessage.EventCategory);
            Assert.Equal(eventSubText, testMessage.EventSubText);
        }

        public class TestMessage : ITimelineMessage
        {
            public Guid Id { get; private set; }  
            public string EventName { get; set; }
            public TimelineCategory EventCategory { get; set; }
            public string EventSubText { get; set; }
            public TimeSpan Offset { get; set; }
            public TimeSpan Duration { get; set; }
            public DateTime StartTime { get; set; }
        }
    }
}
