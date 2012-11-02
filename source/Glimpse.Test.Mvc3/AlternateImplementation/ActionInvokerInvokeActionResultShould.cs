using System;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerInvokeActionResultShould
    {
        [Fact]
        public void ImplementProperMethodForAsyncAndNonAsyncActionInvokers()
        {
            var implementation = new ActionInvoker.InvokeActionResult<ControllerActionInvoker>();

            Assert.Equal("InvokeActionResult", implementation.MethodToImplement.Name);


            var asyncImplementation = new ActionInvoker.InvokeActionResult<AsyncControllerActionInvoker>();

            Assert.Equal("InvokeActionResult", asyncImplementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStrategy);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);

            var implementation = new ActionInvoker.InvokeActionResult<ControllerActionInvoker>();
            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            brokerMock.Verify(b=>b.Publish(It.IsAny<ActionInvoker.InvokeActionResult<ControllerActionInvoker>.Message>()), Times.Never());
        }

        [Fact]
        public void PublishMessageWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStrategy);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);
            contextMock.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext
                                                                    {
                                                                        Controller = new DummyAsyncController(),
                                                                    },
                                                                    new ContentResult(),
                                                            });

            var implementation = new ActionInvoker.InvokeActionResult<ControllerActionInvoker>();
            implementation.NewImplementation(contextMock.Object);

            brokerMock.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionResult<ControllerActionInvoker>.Message>()));
        }

        [Fact]
        public void PublishTimerResultWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Time(It.IsAny<Action>())).Returns(new TimerResult());

            Func<IExecutionTimer> timerStrategy = () => timerMock.Object;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStrategy);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);
            contextMock.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext
                                                                    {
                                                                        Controller = new DummyAsyncController(),
                                                                    },
                                                                    new ContentResult(),
                                                            });

            var implementation = new ActionInvoker.InvokeActionResult<ControllerActionInvoker>();
            implementation.NewImplementation(contextMock.Object);

            brokerMock.Verify(b => b.Publish(It.IsAny<TimerResultMessage>()));
        }
    }
}