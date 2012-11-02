using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewRenderShould
    {
        [Fact]
        public void SetProperties()
        {
            var render = new View.Render();

            Assert.NotNull(render.MethodToImplement);
        }

        [Fact]
        public void PublishMessagesWithOnPolicy()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            Func<IExecutionTimer> timer = () => timerMock.Object;
            Func<RuntimePolicy> policy = () => RuntimePolicy.On;

            var render = new View.Render();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(GetArguments());
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timer);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policy);

            var mixinMock = new Mock<IViewCorrelationMixin>();
            mixinMock.Setup(m => m.ViewName).Returns("astring");
            contextMock.Setup(c => c.Proxy).Returns(mixinMock.Object);
            
            render.NewImplementation(contextMock.Object);

            timerMock.Verify(t=>t.Time(It.IsAny<Action>()));
            brokerMock.Verify(b=>b.Publish(It.IsAny<View.Render.Message>()));
            brokerMock.Verify(b=>b.Publish(It.IsAny<TimerResultMessage>()));
        }

        [Fact]
        public void ProceedWithOffPolicy()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            Func<IExecutionTimer> timer = () => timerMock.Object;
            Func<RuntimePolicy> policy = () => RuntimePolicy.Off;

            var render = new View.Render();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timer);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policy);

            render.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            timerMock.Verify(t=>t.Time(It.IsAny<Action>()), Times.Never());
        }

        private object[] GetArguments()
        {
            var viewContext = new ViewContext();
            var textWriter = new StringWriter();
            return new object[] { viewContext, textWriter };
        }
    }
}