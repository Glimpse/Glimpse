using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
{
    public class LoggingElementShould
    {
        [Fact]
        public void ReturnDefaultLoggingLevel()
        {
            var element = new LoggingElement();

            Assert.Equal(LoggingLevel.Off, element.Level);
        }

        [Fact]
        public void GetSetLevel()
        {
            var level = LoggingLevel.Trace;

            var element = new LoggingElement();

            element.Level = level;

            Assert.Equal(level, element.Level);
        }
    }
}