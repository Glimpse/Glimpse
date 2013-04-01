using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Inspector;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Inspector
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
            Assert.IsAssignableFrom<IInspector>(sut);
        }

        [Theory, AutoMock]
        public void Setup(ViewEngineInspector sut, IInspectorContext context, IViewEngine viewEngine)
        {
            context.ProxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IViewEngine>(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>())).Returns(viewEngine);

            sut.Setup(context);

            context.ProxyFactory.Verify(pf => pf.WrapInterface(It.IsAny<IViewEngine>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>()), Times.AtLeastOnce());
        }
    }
}