using System;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.WebApi.Core.AlternateType
{
    public class IHttpRouteConstraintMatchShould
    {
        [Fact]
        public void ReturnProperMethodToImplement()
        {
            var impl = new IHttpRouteConstraint.Match();

            Assert.Equal("Match", impl.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ReturnWhenRuntimePolicyIsOff(IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var impl = new IHttpRouteConstraint.Match();

            impl.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessageWhenExecuted([Frozen] IExecutionTimer timer, IAlternateMethodContext context, System.Web.Http.Routing.HttpRouteValueDictionary values)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { (System.Web.HttpContextBase)null, new System.Web.Http.Routing.HttpRoute("Test", values), (string)null, (System.Web.Http.Routing.HttpRouteValueDictionary)null, System.Web.Http.Routing.HttpRouteDirection.UriGeneration });
            context.Setup(c => c.ReturnValue).Returns(true);
            context.Setup(c => c.InvocationTarget).Returns(new System.Web.Http.Routing.HttpRoute("Test", null));

            var impl = new IHttpRouteConstraint.Match();

            impl.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<IHttpRouteConstraint.Match.Message>()));
        }
    }
}
