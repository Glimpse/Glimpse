using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using System.Web.Routing;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Ploeh.AutoFixture.Xunit;
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
        public void ProxyActionInvokerIfAsyncControllerFound([Frozen] IProxyFactory proxyFactory, ControllerFactory.CreateController sut, IAlternateImplementationContext context, RequestContext requestContext, string controllerName)
        {
            context.Setup(c => c.ReturnValue).Returns(new DummyAsyncController());
            context.Setup(c => c.Arguments).Returns(new object[] { requestContext, controllerName });
            proxyFactory.Setup(p => p.IsWrapClassEligible(It.IsAny<Type>())).Returns(true);

            sut.NewImplementation(context);

            proxyFactory.Verify(p => p.WrapClass(It.IsAny<AsyncControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>>>(), It.IsAny<IEnumerable<object>>(), null));
        }

        [Theory, AutoMock]
        public void ProxyActionInvokerIfControllerFound([Frozen] IProxyFactory proxyFactory, ControllerFactory.CreateController sut, IAlternateImplementationContext context, RequestContext requestContext, string controllerName)
        {
            context.Setup(c => c.ReturnValue).Returns(new DummyController());
            context.Setup(c => c.Arguments).Returns(new object[] { requestContext, controllerName });
            proxyFactory.Setup(p => p.IsWrapInterfaceEligible(It.IsAny<Type>())).Returns(true);

            sut.NewImplementation(context);

            proxyFactory.Verify(p => p.WrapInterface(It.IsAny<ControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<ControllerActionInvoker>>>(), It.IsAny<IEnumerable<object>>()));
        }
    }
}