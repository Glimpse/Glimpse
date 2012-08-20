using System;
using System.Web.Routing;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc3.AlternateImplementation;
using Moq;

namespace Glimpse.Test.Mvc3.Tester
{
    public class ControllerFactoryCreateControllerTester : ControllerFactory.CreateController
    {
        public Mock<IAlternateImplementationContext> ContextMock { get; set; }
        public Mock<IMessageBroker> MessageBrokerMock { get; set; }

        private ControllerFactoryCreateControllerTester(Func<RuntimePolicy> runtimePolicyStrategy, Mock<IMessageBroker> messageBrokerMock):base(runtimePolicyStrategy, messageBrokerMock.Object)
        {
            ContextMock = new Mock<IAlternateImplementationContext>();
            ContextMock.Setup(c => c.Arguments).Returns(new object[]{new RequestContext(), "a controller name"});
            MessageBrokerMock = messageBrokerMock;
        }

        public static ControllerFactoryCreateControllerTester Create()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var messageBrokerMock = new Mock<IMessageBroker>();

            return new ControllerFactoryCreateControllerTester(runtimePolicyStrategy, messageBrokerMock);
        }
    }
}