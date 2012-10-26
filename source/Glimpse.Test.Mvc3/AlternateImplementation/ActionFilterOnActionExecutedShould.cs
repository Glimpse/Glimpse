using System;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionFilterOnActionExecutedShould
    {
        [Fact]
        public void ReturnProperMethodToImplement()
        {
            var impl = new ActionFilter.OnActionExecuted();

            Assert.Equal("OnActionExecuted", impl.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ReturnWhenRuntimePolicyIsOff(IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var impl = new ActionFilter.OnActionExecuted();

            impl.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessageWhenExecuted([Frozen] IExecutionTimer timer, ActionExecutedContext argument, IAlternateImplementationContext context)
        {
            context.Setup(c => c.Arguments).Returns(new[] { argument });

            var impl = new ActionFilter.OnActionExecuted();

            impl.NewImplementation(context);

            timer.Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ActionFilter.OnActionExecuted.Message>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<TimerResultMessage>()));
        }
    }
}