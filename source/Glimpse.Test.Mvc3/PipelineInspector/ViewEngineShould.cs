using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.PipelineInspector;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.PipelineInspector
{
    public class ViewEngineShould
    {
        public ViewEngineShould()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new WebFormViewEngine());
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        [Fact]
        public void Construct()
        {
            var sut = new ViewEngineInspector();

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IPipelineInspector>(sut);
        }

        [Theory, AutoMock]
        public void Setup(ViewEngineInspector sut, IPipelineInspectorContext context, IViewEngine viewEngine)
        {
            context.ProxyFactory.Setup(pf => pf.IsProxyable(It.IsAny<object>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.CreateProxy(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>(), null, null)).Returns(viewEngine);

            sut.Setup(context);

            context.ProxyFactory.Verify(pf => pf.CreateProxy(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateImplementation<IViewEngine>>>(), null, null), Times.AtLeastOnce());
        }
    }
}