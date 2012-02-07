using System;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class NullLoggerShould
    {
        [Fact]
        public void DoNothingWithoutException()
        {
            var logger = new NullLogger();
            var message = "testing";
            var exception = new Exception("testing");

            logger.Trace(message);
            logger.Debug(message);
            logger.Info(message);
            logger.Warn(message);
            logger.Error(message);
            logger.Fatal(message);

            logger.Trace(message, exception);
            logger.Debug(message, exception);
            logger.Info(message, exception);
            logger.Warn(message, exception);
            logger.Error(message, exception);
            logger.Fatal(message, exception);
        }
    }
}