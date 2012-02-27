using System.Diagnostics;
using System.Threading;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class TimerShould
    {
        [Fact]
        public void TimeFunction()
        {
            var waitTime = 33;
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var result = timer.Time(() => Thread.Sleep(waitTime));

            Assert.NotNull(result);
            Assert.True(result.Duration.Milliseconds >= waitTime-2, result.Duration.Milliseconds.ToString() + " not greater than " + waitTime.ToString());//-2 since we get rounding issues sometimes that makes this test fail
        }

        [Fact]
        public void TimeMethod()
        {
            var waitTime = 33;
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var result = timer.Time(() =>
                                        {
                                            Thread.Sleep(waitTime);
                                            return "string".ToUpper();
                                        });

            Assert.NotNull(result);
            Assert.True(result.Duration.Milliseconds >= waitTime);
            Assert.Equal("STRING", result.Result);
        }

        [Fact]
        public void ConstructWithRunningStopwatch()
        {
            var stopwatch = Stopwatch.StartNew();
            var timer = new ExecutionTimer(stopwatch);

            Assert.NotNull(timer);
            Assert.Equal(stopwatch, timer.Stopwatch);
            Assert.True(timer.Stopwatch.IsRunning);
        }

        [Fact]
        public void ConstructWithNonRunningStopwatch()
        {
            var stopwatch = new Stopwatch();
            var timer = new ExecutionTimer(stopwatch);

            Assert.NotNull(timer);
            Assert.Equal(stopwatch, timer.Stopwatch);
            Assert.True(timer.Stopwatch.IsRunning);
        }
    }
}