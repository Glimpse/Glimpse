using System;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ResultFilterOnResultExecutingShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var sut = new ResultFilter.OnResultExecuting();

            Assert.Equal("OnResultExecuting", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var sut = new ResultFilter.OnResultExecuting();
            sut.NewImplementation(context);
            
            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedAndPublishMessageWithRuntimePolicyOn(ResultExecutingContext argument, IAlternateImplementationContext context)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { argument });

            var sut = new ResultFilter.OnResultExecuting();
            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<TimerResultMessage>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ResultFilter.OnResultExecuting.Message>()));
        }
    }
}