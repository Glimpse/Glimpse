using System;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverShould
    {
        [Fact]
        public void ReturnAllMethods()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            var brokerMock = new Mock<IMessageBroker>();

            var allMethods = DependencyResolver.AllMethods(policyStrategy, brokerMock.Object);

            Assert.NotEmpty(allMethods);
        }
    }
}
