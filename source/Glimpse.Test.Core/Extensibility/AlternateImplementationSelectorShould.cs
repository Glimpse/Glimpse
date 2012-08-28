using System;
using Castle.DynamicProxy;
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
            var alternateMock1 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof (IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof (AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var selector = new AlternateImplementationSelector<IDisposable>();
            var interceptors = new IInterceptor[]
                                   {
                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock1.Object, loggerMock.Object),
                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock2.Object, loggerMock.Object),
                                   };

            var result = selector.SelectInterceptors(null, typeof (IDisposable).GetMethod("Dispose"), interceptors);

            Assert.Equal(1, result.Length);
        }

        [Fact]
        public void ReturnEmptyArrayWithoutMatch()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var interceptors = new IInterceptor[]
                                    {
                                        new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock1.Object, loggerMock.Object), new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock2.Object, loggerMock.Object),
                                    };
            var selector = new AlternateImplementationSelector<IDisposable>();

            var result = selector.SelectInterceptors(null, typeof(AlternateImplementationSelectorShould).GetMethod("ReturnEmptyArrayWithoutMatch"), interceptors);

            Assert.Empty(result);
        }
    }
}