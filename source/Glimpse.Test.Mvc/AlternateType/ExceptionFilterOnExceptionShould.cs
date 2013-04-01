using System;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ExceptionFilterOnExceptionShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var sut = new ExceptionFilter.OnException();

            Assert.Equal("OnException", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            var sut = new ExceptionFilter.OnException();
            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<MessageBase>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedAndPublishMessagesWithRuntimePolicyOn(IAlternateMethodContext context, ExceptionContext argument)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { argument });

            var sut = new ExceptionFilter.OnException();
            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ExceptionFilter.OnException.Message>()));
        }
    }
}