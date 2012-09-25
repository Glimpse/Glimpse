using System;
using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.AlternateImplementation;
using Moq;

namespace Glimpse.Test.Mvc3.Tester
{
    public class ControllerFactoryCreateControllerTester : ControllerFactory.CreateController
    {
        public Mock<IAlternateImplementationContext> ContextMock { get; set; }
        public Mock<IMessageBroker> MessageBrokerMock { get; set; }
        public Mock<IProxyFactory> ProxyFactoryMock { get; set; }
        public Mock<IExecutionTimer> TimerMock { get; set; }

        private ControllerFactoryCreateControllerTester(Func<RuntimePolicy> runtimePolicyStrategy, Mock<IMessageBroker> messageBrokerMock, Mock<IProxyFactory> proxyMock, Mock<IExecutionTimer> timerMock):base(runtimePolicyStrategy, messageBrokerMock.Object, proxyMock.Object, ()=>timerMock.Object)
        {
            ContextMock = new Mock<IAlternateImplementationContext>();
            ContextMock.Setup(c => c.Arguments).Returns(new object[]{new RequestContext(), "a controller name"});
            MessageBrokerMock = messageBrokerMock;
            ProxyFactoryMock = proxyMock;
            ProxyFactoryMock.Setup(p => p.IsProxyable(It.IsAny<IActionInvoker>())).Returns(true);
            TimerMock = timerMock;
        }

        public static ControllerFactoryCreateControllerTester Create()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var messageBrokerMock = new Mock<IMessageBroker>();
            var proxyMock = new Mock<IProxyFactory>();
            var timerMock = new Mock<IExecutionTimer>();

            return new ControllerFactoryCreateControllerTester(runtimePolicyStrategy, messageBrokerMock, proxyMock, timerMock);
        }
    }
}