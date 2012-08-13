using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Message;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateImplementation
{
    public class ViewRenderShould
    {
        [Fact]
        public void SetProperties()
        {
            var brokerMock = new Mock<IMessageBroker>();
            Func<IExecutionTimer> timer = () =>
                                              {
                                                  var mock = new Mock<IExecutionTimer>();
                                                  return mock.Object;
                                              };
            Func<RuntimePolicy> policy = () => RuntimePolicy.On;

            var render = new View.Render(brokerMock.Object, timer, policy);

            Assert.Equal(brokerMock.Object, render.MessageBroker);
            Assert.Equal(timer, render.TimerStrategy);
            Assert.Equal(policy, render.RuntimePolicyStrategy);
            Assert.NotNull(render.MethodToImplement);
        }

        [Fact]
        public void PublishMessagesWithOnPolicy()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var timerMock = new Mock<IExecutionTimer>();
            Func<IExecutionTimer> timer = () => timerMock.Object;
            Func<RuntimePolicy> policy = () => RuntimePolicy.On;

            var render = new View.Render(brokerMock.Object, timer, policy);

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Arguments).Returns(GetArguments());
            var mixinMock = new Mock<View.Render.IMixin>();
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

            var render = new View.Render(brokerMock.Object, timer, policy);

            var contextMock = new Mock<IAlternateImplementationContext>();

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