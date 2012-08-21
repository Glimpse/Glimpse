using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.PipelineInspector;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.PipelineInspector
{
    public class ExecutionShould
    {
        [Fact]
        public void ContinueIfUnableToProxyControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(DefaultControllerFactory));

            var factoryMock = new Mock<IProxyFactory>();
            factoryMock.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(false);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);

            var inspector = new Execution();

            inspector.Setup(contextMock.Object);

            Assert.IsType<DefaultControllerFactory>(ControllerBuilder.Current.GetControllerFactory());
        }

        [Fact]
        public void ProxyControllerFactory()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(DefaultControllerFactory));

            var controllerFactoryMock = new Mock<IControllerFactory>();


            var loggerMock = new Mock<ILogger>();

            var factoryMock = new Mock<IProxyFactory>();
            factoryMock.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(true);
            factoryMock.Setup(f =>f.CreateProxy(It.IsAny<IControllerFactory>(),It.IsAny<IEnumerable<IAlternateImplementation<IControllerFactory>>>())).Returns(controllerFactoryMock.Object);
            factoryMock.Setup(f =>f.CreateProxy(It.IsAny<IDependencyResolver>(),It.IsAny<IEnumerable<IAlternateImplementation<IDependencyResolver>>>())).Returns(new Mock<IDependencyResolver>().Object);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(loggerMock.Object);

            var inspector = new Execution();

            inspector.Setup(contextMock.Object);

            Assert.Equal(ControllerBuilder.Current.GetControllerFactory(), controllerFactoryMock.Object);
            loggerMock.Verify(l => l.Debug(It.Is<string>(s => s.Contains("IControllerFactory")), It.IsAny<object[]>()));
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

            var inspector = new Execution();

            inspector.Setup(contextMock.Object);

            Assert.Equal(resolverMock.Object, DependencyResolver.Current);
        }

        [Fact]
        public void ProxyDependencyResolver()
        {
            var resolverMock = new Mock<IDependencyResolver>();
            DependencyResolver.SetResolver(resolverMock.Object);

            var factoryMock = new Mock<IProxyFactory>();
            factoryMock.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(true);
            factoryMock.Setup(f =>f.CreateProxy(It.IsAny<IDependencyResolver>(),It.IsAny<IEnumerable<IAlternateImplementation<IDependencyResolver>>>())).Returns(resolverMock.Object);
            factoryMock.Setup(f =>f.CreateProxy(It.IsAny<IControllerFactory>(),It.IsAny<IEnumerable<IAlternateImplementation<IControllerFactory>>>())).Returns(new Mock<IControllerFactory>().Object);

            var loggerMock = new Mock<ILogger>();

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(loggerMock.Object);

            var inspector = new Execution();

            inspector.Setup(contextMock.Object);

            Assert.Equal(resolverMock.Object, DependencyResolver.Current);
            loggerMock.Verify(l=>l.Debug(It.Is<string>(s=>s.Contains("IDependencyResolver")), It.IsAny<object[]>()));
        }
    }
}