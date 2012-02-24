using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.PipelineInspector;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.PipelineInspector
{
    public class ViewEngineShould:IDisposable
    {
        [Fact]
        public void Construct()
        {
            var inspector = new ViewEngine();

            Assert.NotNull(inspector);
            Assert.NotNull(inspector as IPipelineInspector);
        }

        [Fact]
        public void Setup()
        {
            var proxyFactoryMock = new Mock<IProxyFactory>();
            proxyFactoryMock.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            proxyFactoryMock.Setup(pf => pf.CreateProxy<IViewEngine>(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>())).Returns(new Mock<IViewEngine>().Object);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(proxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(new Mock<ILogger>().Object);

            var viewEngine = new ViewEngine();

            viewEngine.Setup(contextMock.Object);

            proxyFactoryMock.Verify(pf=>pf.CreateProxy(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>()), Times.AtLeastOnce());
        }

        [Fact]
        public void Teardown()
        {
            var proxyFactoryMock = new Mock<IProxyFactory>();
            proxyFactoryMock.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            proxyFactoryMock.Setup(pf => pf.CreateProxy<IViewEngine>(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>())).Returns(new Mock<IViewEngine>().Object);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(proxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(new Mock<ILogger>().Object);

            Assert.True(ViewEngines.Engines.Any(e=>e.GetType() == typeof(RazorViewEngine)));

            var viewEngine = new ViewEngine();

            viewEngine.Setup(contextMock.Object);

            Assert.False(ViewEngines.Engines.Any(e=>e.GetType() == typeof(RazorViewEngine)));
            
            viewEngine.Teardown(contextMock.Object);

            Assert.True(ViewEngines.Engines.Any(e=>e.GetType() == typeof(RazorViewEngine)));
        }

        [Fact]
        public void NotClearOutViewEnginesIfTeardownCalledBeforeSetup()
        {
            var proxyFactoryMock = new Mock<IProxyFactory>();
            proxyFactoryMock.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            proxyFactoryMock.Setup(pf => pf.CreateProxy<IViewEngine>(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>())).Returns(new Mock<IViewEngine>().Object);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(proxyFactoryMock.Object);

            var viewEngine = new ViewEngine();

            viewEngine.Teardown(contextMock.Object);

            Assert.True(ViewEngines.Engines.Any(e => e.GetType() == typeof(RazorViewEngine)));
        }

        public void Dispose()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebFormViewEngine());
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}