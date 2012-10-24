using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Mvc3.TestDoubles;
using Glimpse.Test.Mvc3.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerShould:IDisposable
    {
        private ControllerFactoryCreateControllerTester tester;
        public ControllerFactoryCreateControllerTester Tester
        {
            get { return tester ?? (tester = ControllerFactoryCreateControllerTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Tester = null;
        }

        [Fact]
        public void SetMethodToImplement()
        {
            var result = Tester.MethodToImplement;

            Assert.Equal("CreateController", result.Name);
        }

        [Fact]
        public void ProceedImmediatlyIfRuntimePolicyOff()
        {
            Tester.ContextMock.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            Tester.NewImplementation(Tester.ContextMock.Object);

            Tester.ContextMock.Verify(c=>c.Proceed());
            Tester.MessageBrokerMock.Verify(mb => mb.Publish(It.IsAny<ControllerFactory.CreateController.Message>()), Times.Never());
        }

        [Fact]
        public void PublishMessageIfRuntimePolicyOn()
        {
            Tester.NewImplementation(Tester.ContextMock.Object);

            Tester.MessageBrokerMock.Verify(mb=>mb.Publish(It.IsAny<ControllerFactory.CreateController.Message>()));
        }

        [Fact]
        public void ProxyActionInvokerIfAsyncControllerFound()
        {
            Tester.ContextMock.Setup(c => c.ReturnValue).Returns(new DummyAsyncController());

            Tester.NewImplementation(Tester.ContextMock.Object);

            Tester.ProxyFactoryMock.Verify(p => p.CreateProxy(It.IsAny<AsyncControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>>>(), It.IsAny<object>()));
        }

        [Fact]
        public void ProxyActionInvokerIfControllerFound()
        {
            Tester.ContextMock.Setup(c => c.ReturnValue).Returns(new DummyController());

            Tester.NewImplementation(Tester.ContextMock.Object);

            Tester.ProxyFactoryMock.Verify(p => p.CreateProxy(It.IsAny<ControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<ControllerActionInvoker>>>()));
        }
    }
}