using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc3.AlternateImplementation;
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
        public void ThrowWithNullRuntimePolicyStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => new ControllerFactory.CreateController(null, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new Mock<IExecutionTimer>().Object));
        }

        [Fact]
        public void ThrowWithNullMessageBroker()
        {
            Assert.Throws<ArgumentNullException>(() => new ControllerFactory.CreateController(() => RuntimePolicy.On, null, new Mock<IProxyFactory>().Object, () => new Mock<IExecutionTimer>().Object));
        }

        [Fact]
        public void ThrowWithNullProxyFactory()
        {
            Assert.Throws<ArgumentNullException>(() => new ControllerFactory.CreateController(() => RuntimePolicy.On, new Mock<IMessageBroker>().Object, null, () => new Mock<IExecutionTimer>().Object));
        }

        [Fact]
        public void ThrowsWithNullTimerStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => new ControllerFactory.CreateController(() => RuntimePolicy.On, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, null));
        }

        [Fact]
        public void ProceedImmediatlyIfRuntimePolicyOff()
        {
            Tester.RuntimePolicyStrategy = ()=> RuntimePolicy.Off;

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
        public void ProxyActionInvokerIfControllerFound()
        {
            Tester.ContextMock.Setup(c => c.ReturnValue).Returns(new DummyAsyncController());

            Tester.NewImplementation(Tester.ContextMock.Object);

            Tester.ProxyFactoryMock.Verify(p => p.CreateProxy(It.IsAny<AsyncControllerActionInvoker>(), It.IsAny<IEnumerable<IAlternateImplementation<AsyncControllerActionInvoker>>>(), It.IsAny<object>()));
        }
    }
}