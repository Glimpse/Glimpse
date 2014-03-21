using System;
using System.Diagnostics.CodeAnalysis;
using Glimpse.WebApi.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.WebApi.AlternateType
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class IHttpRouteGetRouteDataShould : GetRouteDataShould<System.Web.Http.Routing.IHttpRoute>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class HttpRouteGetRouteDataShould : GetRouteDataShould<System.Web.Http.Routing.HttpRoute>
    {
    }

    public abstract class GetRouteDataShould<T>
        where T : System.Web.Http.Routing.IHttpRoute
    {
        [Fact]
        public void ReturnProperMethodToImplement()
        {
            var impl = new IHttpRoute.GetRouteData(typeof(T));

            Assert.Equal("GetRouteData", impl.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ReturnWhenRuntimePolicyIsOff(IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var impl = new IHttpRoute.GetRouteData(typeof(T));

            impl.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessageWhenExecuted([Frozen] IExecutionTimer timer, IAlternateMethodContext context, IRouteNameMixin mixin)
        {
            context.Setup(c => c.Arguments).Returns(new object[5]);
            context.Setup(c => c.ReturnValue).Returns(new System.Web.Http.Routing.HttpRouteData(new System.Web.Http.Routing.HttpRoute()));
            context.Setup(c => c.InvocationTarget).Returns(new System.Web.Http.Routing.HttpRoute("Test", null));
            context.Setup(c => c.Proxy).Returns(mixin);

            var impl = new IHttpRoute.GetRouteData(typeof(T));

            impl.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<IHttpRoute.GetRouteData.Message>()));
        }
    }
}