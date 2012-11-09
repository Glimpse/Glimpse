using System.Linq;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;
using DependencyResolver = Glimpse.Mvc.AlternateImplementation.DependencyResolver;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServicesShould
    {
        [Fact]
        public void Construct()
        {
            var sut = new DependencyResolver.GetServices();

            Assert.NotNull(sut.MethodToImplement);
        }

        [Theory, AutoMock]
        public void ImplementGetServices(DependencyResolver.GetServices sut)
        {
            Assert.Equal("GetServices", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedWithRuntimePolicyOff(DependencyResolver.GetServices sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.Verify(c => c.Arguments, Times.Never());
            context.Verify(c => c.ReturnValue, Times.Never());
        }

        [Theory, AutoMock]
        public void PublishMessageOnGetServices(DependencyResolver.GetServices sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { typeof(string) });
            context.Setup(c => c.ReturnValue).Returns(Enumerable.Empty<object>());

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<DependencyResolver.GetServices.Message>()));
        }
    }
}