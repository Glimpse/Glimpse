using System;
using System.Diagnostics.CodeAnalysis;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions; 

namespace Glimpse.Test.AspNet.AlternateImplementation
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class RouteBaseGetRouteDataShould : GetRouteDataShould<System.Web.Routing.RouteBase>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class RouteGetRouteDataShould : GetRouteDataShould<System.Web.Routing.Route>
    {
    }

    public abstract class GetRouteDataShould<T>
        where T : System.Web.Routing.RouteBase
    {
        [Fact]
        public void ReturnProperMethodToImplement()
        {
            var impl = new RouteBase.GetRouteData(typeof(T));

            Assert.Equal("GetRouteData", impl.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ReturnWhenRuntimePolicyIsOff(IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var impl = new RouteBase.GetRouteData(typeof(T));

            impl.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessageWhenExecuted([Frozen] IExecutionTimer timer, IAlternateImplementationContext context)
        {
            context.Setup(c => c.Arguments).Returns(new object[5]);
            context.Setup(c => c.ReturnValue).Returns(new System.Web.Routing.RouteData());
            context.Setup(c => c.InvocationTarget).Returns(new System.Web.Routing.Route("Test", null));
            context.Setup(c => c.Proxy).Returns(new object());

            var impl = new RouteBase.GetRouteData(typeof(T));

            impl.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<RouteBase.GetRouteData.Message>()));
        }
    }
}