using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class LoggerBaseShould
    {
        [Fact]
        public void CallAbstractTrace()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Trace("message Format", new object[]{"One", 2});

            loggerMock.Verify(l=>l.Trace(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractTraceWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Trace("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Trace(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void CallAbstractDebug()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Debug("message Format", new object[] { "One", 2 });

            loggerMock.Verify(l => l.Debug(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractDebugWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Debug("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Debug(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void CallAbstractInfo()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Info("message Format", new object[] { "One", 2 });

            loggerMock.Verify(l => l.Info(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractInfoWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Info("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Info(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void CallAbstractWarn()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Warn("message Format", new object[] { "One", 2 });

            loggerMock.Verify(l => l.Warn(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractWarnWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Warn("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Warn(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void CallAbstractError()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Error("message Format", new object[] { "One", 2 });

            loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractErrorWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Error("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Error(It.IsAny<string>(), exception), Times.Once());
        }

        [Fact]
        public void CallAbstractFatal()
        {
            var loggerMock = new Mock<LoggerBase>();

            loggerMock.Object.Fatal("message Format", new object[] { "One", 2 });

            loggerMock.Verify(l => l.Fatal(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void CallAbstractFatalWithException()
        {
            var loggerMock = new Mock<LoggerBase>();
            var exception = new DummyException();

            loggerMock.Object.Fatal("message Format", exception, new object[] { "One", 2 });

            loggerMock.Verify(l => l.Fatal(It.IsAny<string>(), exception), Times.Once());
        }
    }
}