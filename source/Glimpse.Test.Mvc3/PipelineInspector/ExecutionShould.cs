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
    public class ExecutionShould
    {
        public ExecutionShould()
        {
            ControllerBuilder.Current.SetControllerFactory(typeof(DefaultControllerFactory));
        }

        [Theory, AutoMock]
        public void ContinueIfUnableToProxyControllerFactory(ExecutionInspector sut, IPipelineInspectorContext context)
        {
            context.ProxyFactory.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(false);

            sut.Setup(context);

            Assert.IsType<DefaultControllerFactory>(ControllerBuilder.Current.GetControllerFactory());
        }

        [Theory, AutoMock]
        public void ProxyControllerFactory(ExecutionInspector sut, IPipelineInspectorContext context, IControllerFactory controllerFactory)
        {
            context.ProxyFactory.Setup(f => f.IsProxyable(It.IsAny<object>())).Returns(true);
            context.ProxyFactory.Setup(f => f.CreateProxy(It.IsAny<IControllerFactory>(), It.IsAny<IEnumerable<IAlternateImplementation<IControllerFactory>>>(), null)).Returns(controllerFactory);

            sut.Setup(context);

            Assert.Equal(ControllerBuilder.Current.GetControllerFactory(), controllerFactory);
            context.Logger.Verify(l => l.Debug(It.Is<string>(s => s.Contains("IControllerFactory")), It.IsAny<object[]>()));
        }
    }
}