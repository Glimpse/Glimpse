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
            var message = "testing {0{";
            var exception = new Exception("testing");
            var parameters = new object[] {"One", 2};

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

            logger.Trace(message, parameters);
            logger.Debug(message, parameters);
            logger.Info(message, parameters);
            logger.Warn(message, parameters);
            logger.Error(message, parameters);
            logger.Fatal(message, parameters);

            logger.Trace(message, exception, parameters);
            logger.Debug(message, exception, parameters);
            logger.Info(message, exception, parameters);
            logger.Warn(message, exception, parameters);
            logger.Error(message, exception, parameters);
            logger.Fatal(message, exception, parameters);
        }
    }
}