using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.PipelineInspector;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.PipelineInspector
{
    public class DependencyInjectionShould
    {
        [Fact]
        public void ProxyDependencyResolver()
        {
            var resolverMock = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(resolverMock.Object);

            var factoryMock = new Mock<IProxyFactory>();
            factoryMock.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(true);
            factoryMock.Setup(f => f.CreateProxy(It.IsAny<IDependencyResolver>(), It.IsAny<IEnumerable<IAlternateImplementation<IDependencyResolver>>>(), null)).Returns(resolverMock.Object);

            var loggerMock = new Mock<ILogger>();

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(loggerMock.Object);

            var inspector = new DependencyInjectionInspector();

            inspector.Setup(contextMock.Object);

            Assert.Equal(resolverMock.Object, DependencyResolver.Current);
            loggerMock.Verify(l => l.Debug(It.Is<string>(s => s.Contains("IDependencyResolver")), It.IsAny<object[]>()));
        }

        [Fact]
        public void ContinueIfUnableToProxyDependencyResolver()
        {
            var resolverMock = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(resolverMock.Object);

            var factoryMock = new Mock<IProxyFactory>();
            factoryMock.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(false);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);

            var inspector = new DependencyInjectionInspector();

            inspector.Setup(contextMock.Object);

            Assert.Equal(resolverMock.Object, DependencyResolver.Current);
        }
    }
}