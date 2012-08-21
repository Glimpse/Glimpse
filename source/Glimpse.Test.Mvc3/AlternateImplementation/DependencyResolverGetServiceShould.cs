using System;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServiceShould
    {
        [Fact]
        public void Construct()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = ()=>RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();
            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetService(runtimePolicyStrategy, brokerMock.Object);

            Assert.Equal(runtimePolicyStrategy, implementation.RuntimePolicyStrategy);
            Assert.Equal(brokerMock.Object, implementation.MessageBroker);
        }

        [Fact]
        public void ImplementGetService()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();
            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetService(runtimePolicyStrategy, brokerMock.Object);

            var methodToImplement = implementation.MethodToImplement;

            Assert.Equal("GetService", methodToImplement.Name);
        }

        [Fact]
        public void ProceedWithRuntimePolicyOff()
        {
            var contextMock = new Mock<IAlternateImplementationContext>();

            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.Off;
            var brokerMock = new Mock<IMessageBroker>();
            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetService(runtimePolicyStrategy, brokerMock.Object);

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.ReturnValue, Times.Never());
            contextMock.Verify(c=>c.Arguments, Times.Never());
        }

        [Fact]
        public void PublishMessageWithRuntimePolicyOn()
        {
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(new object[] {typeof(IController)});
            contextMock.Setup(c => c.ReturnValue).Returns(null);

            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();
            var implementation = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetService(runtimePolicyStrategy, brokerMock.Object);

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            brokerMock.Verify(b => b.Publish(It.IsAny<Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetService.Message>()));
        }
    }
}