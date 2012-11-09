using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerShould
    {
        [Theory, AutoMock]
        public void SetMethodToImplement(ControllerFactory.CreateController sut)
        {
            var result = sut.MethodToImplement;

            Assert.Equal("CreateController", result.Name);
        }

        [Theory, AutoMock]
        public void ProceedImmediatlyIfRuntimePolicyOff(ControllerFactory.CreateController sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ControllerFactory.CreateController.Message>()), Times.Never());
        }

        [Theory, AutoMock]
        public void PublishMessageIfRuntimePolicyOn(ControllerFactory.CreateController sut, IAlternateImplementationContext context, RequestContext requestContext, string controllerName)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { requestContext, controllerName });

            sut.NewImplementation(context);

            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ControllerFactory.CreateController.Message>()));
        }

        [Theory, AutoMock]
        public void ProxyActionInvokerIfAsyncControllerFound(ControllerFactory.CreateController sut, IAlternateImplementationContext context, RequestContext requestContext, string controllerName)
        {
            context.Setup(c => c.ReturnValue).Returns(new DummyAsyncController());
            context.Setup(c => c.Arguments).Returns(new object[] { requestContext, controllerName });
            context.ProxyFactory.Setup(p => p.IsProxyable(It.IsAny<IActionInvoker>())).Returns(true);

            sut.NewImplementation(context);

            context.ProxyFactory.Verify(p => p.CreateProxy(It.IsAny<AsyncControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>>>(), It.IsAny<object>()));
        }

        [Theory, AutoMock]
        public void ProxyActionInvokerIfControllerFound(ControllerFactory.CreateController sut, IAlternateImplementationContext context, RequestContext requestContext, string controllerName)
        {
            context.Setup(c => c.ReturnValue).Returns(new DummyController());
            context.Setup(c => c.Arguments).Returns(new object[] { requestContext, controllerName });
            context.ProxyFactory.Setup(p => p.IsProxyable(It.IsAny<IActionInvoker>())).Returns(true);

            sut.NewImplementation(context);

            context.ProxyFactory.Verify(p => p.CreateProxy(It.IsAny<ControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<ControllerActionInvoker>>>(), null));
        }
    }
}