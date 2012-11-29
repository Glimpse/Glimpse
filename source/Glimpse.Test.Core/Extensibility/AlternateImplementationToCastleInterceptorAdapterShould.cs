using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class AlternateImplementationToCastleInterceptorAdapterShould
    {
        [Fact]
        public void Construct()
        {
            var implementationMock = new Mock<IAlternateImplementation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter(implementationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(implementationMock.Object, adapter.Implementation);
            Assert.Equal(loggerMock.Object, adapter.Logger);
        }

        [Fact]
        public void ThrowWithNullConstructorParameters()
        {
            var implementationMock = new Mock<IAlternateImplementation>();
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(() => new AlternateImplementationToCastleInterceptorAdapter(null, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
            Assert.Throws<ArgumentNullException>(() => new AlternateImplementationToCastleInterceptorAdapter(implementationMock.Object, null, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
        }

        [Fact]
        public void PassThroughMethodToImplement()
        {
            var expected = GetType().GetMethods().First();
            var implementationMock = new Mock<IAlternateImplementation>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter(implementationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.MethodToImplement);
        }

        [Fact]
        public void Intercept()
        {
            var implementationMock = new Mock<IAlternateImplementation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter(implementationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            var invocationMock = new Mock<IInvocation>();

            adapter.Intercept(invocationMock.Object);

            implementationMock.Verify(i => i.NewImplementation(It.IsAny<IAlternateImplementationContext>()));
        }
    }
}