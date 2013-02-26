using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class AlternateTypeSelectorShould
    {
        [Fact]
        public void ReturnMatchingInterceptors()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateMethod>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateMethod>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateTypeSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var selector = new AlternateTypeSelector();
            var interceptors = new IInterceptor[]
                                   {
                                       new AlternateTypeToCastleInterceptorAdapter(alternateMock1.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On),
                                       new AlternateTypeToCastleInterceptorAdapter(alternateMock2.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On)
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
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateTypeSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var interceptors = new IInterceptor[]
                                    {
                                        new AlternateTypeToCastleInterceptorAdapter(alternateMock1.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On), new AlternateTypeToCastleInterceptorAdapter(alternateMock2.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On)
                                    };
            var selector = new AlternateTypeSelector();

            var result = selector.SelectInterceptors(null, typeof(AlternateTypeSelectorShould).GetMethod("ReturnEmptyArrayWithoutMatch"), interceptors);

            Assert.Empty(result);
        }
    }
}