using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class AlternateImplementationSelectorShould
    {
        [Fact]
        public void ReturnMatchingInterceptors()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateMethod>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateMethod>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var selector = new AlternateImplementationSelector();
            var interceptors = new IInterceptor[]
                                   {
                                       new AlternateImplementationToCastleInterceptorAdapter(alternateMock1.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On),
                                       new AlternateImplementationToCastleInterceptorAdapter(alternateMock2.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On)
                                   };

            var result = selector.SelectInterceptors(null, typeof(IDisposable).GetMethod("Dispose"), interceptors);

            Assert.Equal(1, result.Length);
        }

        [Fact]
        public void ReturnEmptyArrayWithoutMatch()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateMethod>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateMethod>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var interceptors = new IInterceptor[]
                                    {
                                        new AlternateImplementationToCastleInterceptorAdapter(alternateMock1.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On), new AlternateImplementationToCastleInterceptorAdapter(alternateMock2.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On)
                                    };
            var selector = new AlternateImplementationSelector();

            var result = selector.SelectInterceptors(null, typeof(AlternateImplementationSelectorShould).GetMethod("ReturnEmptyArrayWithoutMatch"), interceptors);

            Assert.Empty(result);
        }
    }
}