using System;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class AlternateImplementationToCastleInterceptorAdapterShould
    {
        [Fact]
        public void Construct()
        {
            var implementationMock = new Mock<IAlternateImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            Assert.Equal(implementationMock.Object, adapter.Implementation);
            Assert.Equal(loggerMock.Object, adapter.Logger);
        }

        [Fact]
        public void ThrowWithNullConstructorParameters()
        {
            var implementationMock = new Mock<IAlternateImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(()=>new AlternateImplementationToCastleInterceptorAdapter<ITab>(null, loggerMock.Object));
            Assert.Throws<ArgumentNullException>(() => new AlternateImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, null));
        }

        [Fact]
        public void PassThroughMethodToImplement()
        {
            var expected = GetType().GetMethods().First();
            var implementationMock = new Mock<IAlternateImplementation<ITab>>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            Assert.Equal(expected, adapter.MethodToImplement);
        }

        [Fact]
        public void Intercept()
        {
            var implementationMock = new Mock<IAlternateImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            var invocationMock = new Mock<IInvocation>();

            adapter.Intercept(invocationMock.Object);

            implementationMock.Verify(i=>i.NewImplementation(It.IsAny<IAlternateImplementationContext>()));
        }
    }
}