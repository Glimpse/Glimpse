using System;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;
using DependencyResolver = Glimpse.Mvc.AlternateImplementation.DependencyResolver;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServiceShould
    {
        [Fact]
        public void Construct()
        {
            var implementation = new DependencyResolver.GetService();

            Assert.NotNull(implementation.MethodToImplement);
        }

        [Fact]
        public void ImplementGetService()
        {
            var implementation = new DependencyResolver.GetService();

            var methodToImplement = implementation.MethodToImplement;

            Assert.Equal("GetService", methodToImplement.Name);
        }

        [Fact]
        public void ProceedWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.Off;
            var brokerMock = new Mock<IMessageBroker>();
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimePolicyStrategy);
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);

            var implementation = new DependencyResolver.GetService();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.ReturnValue, Times.Never());
            contextMock.Verify(c=>c.Arguments, Times.Never());
        }

        [Fact]
        public void PublishMessageWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();
            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(new object[] {typeof(IController)});
            contextMock.Setup(c => c.ReturnValue).Returns(null);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimePolicyStrategy);
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);

            var implementation = new DependencyResolver.GetService();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            brokerMock.Verify(b => b.Publish(It.IsAny<DependencyResolver.GetService.Message>()));
        }
    }
}