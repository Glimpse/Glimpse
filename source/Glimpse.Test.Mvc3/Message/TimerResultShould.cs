using System;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;
using Xunit;

namespace Glimpse.Test.Mvc3.Message
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

            var result = new TimerResultMessage(functionTimerResult, expectedEventName, expectedCategory);

            Assert.Equal(expectedCategory, result.EventCategory);
            Assert.Equal(expectedDuration, result.Duration);
            Assert.Equal(expectedEventName, result.EventName);
            Assert.Equal(expectedOffset, result.Offset);
        }
    }
}