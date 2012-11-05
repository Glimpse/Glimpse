using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.PipelineInspector;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.PipelineInspector
{
    public class ViewEngineShould:IDisposable
    {
        [Fact]
        public void Construct()
        {
            var inspector = new ViewEngineInspector();

            Assert.NotNull(inspector);
            Assert.NotNull(inspector as IPipelineInspector);
        }

        [Fact]
        public void Setup()
        {
            var proxyFactoryMock = new Mock<IProxyFactory>();
            proxyFactoryMock.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            proxyFactoryMock.Setup(pf => pf.CreateProxy<IViewEngine>(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>(), null)).Returns(new Mock<IViewEngine>().Object);

            var contextMock = new Mock<IPipelineInspectorContext>();
            contextMock.Setup(c => c.ProxyFactory).Returns(proxyFactoryMock.Object);
            contextMock.Setup(c => c.Logger).Returns(new Mock<ILogger>().Object);

            var viewEngine = new ViewEngineInspector();

            viewEngine.Setup(contextMock.Object);

            proxyFactoryMock.Verify(pf => pf.CreateProxy(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>(), null), Times.AtLeastOnce());
        }

        public void Dispose()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebFormViewEngine());
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}