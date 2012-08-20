using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class PipelineInspectorContextShould
    {
        [Fact]
        public void SetPropertiesOnConstruct()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            var context = new PipelineInspectorContext(loggerMock.Object, factoryMock.Object, brokerMock.Object, () => timerMock.Object, ()=>RuntimePolicy.On);

            Assert.NotNull(context);
            Assert.Equal(loggerMock.Object, context.Logger);
            Assert.Equal(factoryMock.Object, context.ProxyFactory);
            Assert.Equal(brokerMock.Object, context.MessageBroker);
            Assert.Equal(timerMock.Object, context.TimerStrategy());
        }

        [Fact]
        public void ThrowExceptionOnConstructWithNullLogger()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            Assert.Throws<ArgumentNullException>(() => new PipelineInspectorContext(null, factoryMock.Object, brokerMock.Object, () => timerMock.Object, () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowExceptionOnConstructWithNullProxyFactory()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            Assert.Throws<ArgumentNullException>(() => new PipelineInspectorContext(loggerMock.Object, null, brokerMock.Object, () => timerMock.Object, () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowExceptionOnConstructWithNullMessageBroker()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            Assert.Throws<ArgumentNullException>(() => new PipelineInspectorContext(loggerMock.Object, factoryMock.Object, null, () => timerMock.Object, () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowExceptionOnConstructWithNullTimerStrategy()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            Assert.Throws<ArgumentNullException>(() => new PipelineInspectorContext(loggerMock.Object, factoryMock.Object, brokerMock.Object, null, () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowExceptionOnConstructWithNullRuntimePolicyStrategy()
        {
            var loggerMock = new Mock<ILogger>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();

            Assert.Throws<ArgumentNullException>(() => new PipelineInspectorContext(loggerMock.Object, factoryMock.Object, brokerMock.Object, () => timerMock.Object, null));
        }
    }
}