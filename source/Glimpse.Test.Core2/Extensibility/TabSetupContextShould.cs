using System;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class TabSetupContextShould
    {
        [Fact]
        public void Construct()
        {
            var loggerMock = new Mock<ILogger>();
            var brokerMock = new Mock<IMessageBroker>();
            var dataStoreMock = new Mock<IDataStore>();

            var context = new TabSetupContext(loggerMock.Object, brokerMock.Object, ()=>dataStoreMock.Object);

            Assert.Equal(loggerMock.Object, context.Logger);
            Assert.Equal(brokerMock.Object, context.MessageBroker);
            Assert.Equal(dataStoreMock.Object, context.GetTabStore());
        }

        [Fact]
        public void ThrowExceptionWithNullLogger()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var dataStoreMock = new Mock<IDataStore>();

            Assert.Throws<ArgumentNullException>(()=> new TabSetupContext(null, brokerMock.Object, () => dataStoreMock.Object));
        }

        [Fact]
        public void ThrowExceptionWithNullMessageBroker()
        {
            var loggerMock = new Mock<ILogger>();
            var dataStoreMock = new Mock<IDataStore>();

            Assert.Throws<ArgumentNullException>(() => new TabSetupContext(loggerMock.Object, null, () => dataStoreMock.Object));
        }

        [Fact]
        public void ThrowExceptionWithNullTabeStoreStrategy()
        {
            var loggerMock = new Mock<ILogger>();
            var brokerMock = new Mock<IMessageBroker>();

            Assert.Throws<ArgumentNullException>(() => new TabSetupContext(loggerMock.Object, brokerMock.Object, null));
        }
    }
}