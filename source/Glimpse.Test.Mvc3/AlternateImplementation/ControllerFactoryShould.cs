using System;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryShould
    {
        [Fact]
        public void ReturnAllAlternateImplementations()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var messageBrokerMock = new Mock<IMessageBroker>();
            var proxyMock = new Mock<IProxyFactory>();
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;

            var implementations = ControllerFactory.AllMethods(runtimePolicyStrategy, messageBrokerMock.Object, proxyMock.Object, timerStrategy);

            Assert.NotEmpty(implementations);
        }
    }
}