using System;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class AlternateMethodImplementationToCastleInterceptorAdapterShould
    {
        [Fact]
        public void Construct()
        {
            var implementationMock = new Mock<IAlternateMethodImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateMethodImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            Assert.Equal(implementationMock.Object, adapter.Implementation);
            Assert.Equal(loggerMock.Object, adapter.Logger);
        }

        [Fact]
        public void ThrowWithNullConstructorParameters()
        {
            var implementationMock = new Mock<IAlternateMethodImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(()=>new AlternateMethodImplementationToCastleInterceptorAdapter<ITab>(null, loggerMock.Object));
            Assert.Throws<ArgumentNullException>(() => new AlternateMethodImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, null));
        }

        [Fact]
        public void PassThroughMethodToImplement()
        {
            var expected = GetType().GetMethods().First();
            var implementationMock = new Mock<IAlternateMethodImplementation<ITab>>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateMethodImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            Assert.Equal(expected, adapter.MethodToImplement);
        }

        [Fact]
        public void Intercept()
        {
            var implementationMock = new Mock<IAlternateMethodImplementation<ITab>>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new AlternateMethodImplementationToCastleInterceptorAdapter<ITab>(implementationMock.Object, loggerMock.Object);

            var invocationMock = new Mock<IInvocation>();

            adapter.Intercept(invocationMock.Object);

            implementationMock.Verify(i=>i.NewImplementation(It.IsAny<IAlternateMethodImplementationContext>()));
        }
    }
}