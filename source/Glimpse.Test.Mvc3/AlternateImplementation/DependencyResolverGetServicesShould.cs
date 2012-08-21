using System;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServicesShould
    {
        [Fact]
        public void Construct()
        {
            Func<RuntimePolicy> policyStrategy = ()=> RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();

            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices(policyStrategy, brokerMock.Object);

            Assert.Equal(policyStrategy, implementation.RuntimePolicyStrategy);
            Assert.Equal(brokerMock.Object, implementation.MessageBroker);
        }

        [Fact]
        public void ImplementGetServices()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();

            IAlternateImplementation<IDependencyResolver> implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices(policyStrategy, brokerMock.Object);

            Assert.Equal("GetServices", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();

            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices(policyStrategy, brokerMock.Object);

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

            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices(policyStrategy, brokerMock.Object);

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            brokerMock.Verify(b => b.Publish(It.IsAny<Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices.Message>()));
        }
    }
}