using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Message
{
    public class TimedMessageShould
    {
        [Theory, AutoMock]
        public void ShouldBeAbleToBuildWithFactory(TimerResult timerResult)
        {
            var testMessage = new TestMessage().AsTimedMessage(timerResult);

            Assert.Equal(timerResult.Offset, testMessage.Offset);
            Assert.Equal(timerResult.Duration, testMessage.Duration);
            Assert.Equal(timerResult.StartTime, testMessage.StartTime);
        }

        public class TestMessage : ITimedMessage
        {
            public Guid Id { get; private set; }
            public TimeSpan Offset { get; set; }
            public TimeSpan Duration { get; set; }
            public DateTime StartTime { get; set; } 
        }
    }
}
