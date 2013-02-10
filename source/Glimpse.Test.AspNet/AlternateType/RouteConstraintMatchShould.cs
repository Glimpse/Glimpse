using System;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.AlternateType
{
    public class RouteConstraintMatchShould
    {
        [Fact]
        public void ReturnProperMethodToImplement()
        {
            var impl = new RouteConstraint.Match();

            Assert.Equal("Match", impl.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ReturnWhenRuntimePolicyIsOff(IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var impl = new RouteConstraint.Match();

            impl.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessageWhenExecuted([Frozen] IExecutionTimer timer, IAlternateMethodContext context, System.Web.Routing.IRouteHandler handler)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { (System.Web.HttpContextBase)null, new System.Web.Routing.Route("Test", handler), (string)null, (System.Web.Routing.RouteValueDictionary)null, System.Web.Routing.RouteDirection.IncomingRequest });
            context.Setup(c => c.ReturnValue).Returns(true);
            context.Setup(c => c.InvocationTarget).Returns(new System.Web.Routing.Route("Test", null));

            var impl = new RouteConstraint.Match();

            impl.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<RouteConstraint.Match.Message>()));
        }
    }
}
