using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Message
{
    public class TimerResultMessageShould
    {
        [Theory, AutoMock]
        public void Construct(TimerResult timerResult, string expectedEventName, string expectedCategory)
        {
            var sut = new TimerResultMessage(timerResult, expectedEventName, expectedCategory);

            Assert.Equal(expectedCategory, sut.EventCategory);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(expectedEventName, sut.EventName);
            Assert.Equal(timerResult.Offset, sut.Offset);
        }
    }
}