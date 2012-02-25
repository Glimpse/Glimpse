using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.Message;
using Xunit;
using TimerResult = Glimpse.Core2.Extensibility.TimerResult;

namespace Glimpse.Test.Mvc.Message
{
    public class TimerResultShould
    {
        [Fact]
        public void Construct()
        {
            var expectedDuration = TimeSpan.FromMilliseconds(300);
            var expectedOffset = 55;
            var expectedEventName = "any string";
            var expectedCategory = "any string";

            var functionTimerResult = new TimerResult {Duration = expectedDuration, Offset = expectedOffset};

            var result = new Glimpse.Mvc.Message.TimerResultMessage(functionTimerResult, expectedEventName, expectedCategory);

            Assert.Equal(expectedCategory, result.EventCategory);
            Assert.Equal(expectedDuration, result.Duration);
            Assert.Equal(expectedEventName, result.EventName);
            Assert.Equal(expectedOffset, result.Offset);
        }
    }
}