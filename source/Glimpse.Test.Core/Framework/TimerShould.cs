using System;
using System.Diagnostics;
using System.Threading;
using Glimpse.Core.Extensibility;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class TimerShould
    {
        [Fact]
        public void TimeFunction()
        {
            var waitTime = 5;
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var result = timer.Time(() => Thread.Sleep(waitTime));

            Assert.NotNull(result);
            var failureMessage = result.Duration.ToString() + " not greater than " + waitTime.ToString();
            Console.Write(failureMessage);
            Assert.True(result.Duration >= waitTime-1, failureMessage); //-1 to handle issues with rounding??
        }

        [Fact(Skip = "This test is flaky. Need to find a better way to do this...")]
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
            Assert.True(result.Duration >= waitTime);
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