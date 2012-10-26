using System;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;
using DependencyResolver = Glimpse.Mvc.AlternateImplementation.DependencyResolver;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServicesShould
    {
        [Fact]
        public void Construct()
        {
            var implementation = new DependencyResolver.GetServices();

            Assert.NotNull(implementation.MethodToImplement);
        }

        [Fact]
        public void ImplementGetServices()
        {
            IAlternateImplementation<IDependencyResolver> implementation = new DependencyResolver.GetServices();

            Assert.Equal("GetServices", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);

            var implementation = new DependencyResolver.GetServices();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.Arguments, Times.Never());
            contextMock.Verify(c=>c.ReturnValue, Times.Never());
        }

        [Fact]
        public void PublishMessageOnGetServices()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(new object[] {typeof (string)});
            contextMock.Setup(c => c.ReturnValue).Returns(Enumerable.Empty<object>());
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);

            var implementation = new DependencyResolver.GetServices();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            brokerMock.Verify(b => b.Publish(It.IsAny<DependencyResolver.GetServices.Message>()));
        }
    }
}