using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AsyncActionInvokerEndInvokeActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var implementation = new AsyncActionInvoker.EndInvokeActionMethod();

            Assert.Equal("EndInvokeActionMethod", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedThenReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> runtimeStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimeStrategy);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStategy);

            var implementation = new AsyncActionInvoker.EndInvokeActionMethod();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.Proxy, Times.Never());
        }

        [Fact]
        public void PublishMessageWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> runtimeStrategy = () => RuntimePolicy.On;

            var timerMock = new Mock<IExecutionTimer>();
            timerMock.Setup(t => t.Stop(It.IsAny<long>())).Returns(new TimerResult());

            Func<IExecutionTimer> timerStategy = () => timerMock.Object;
            var brokerMock = new Mock<IMessageBroker>();

            var controllerDescriptorMock = new Mock<ControllerDescriptor>();
            controllerDescriptorMock.Setup(cd => cd.ControllerName).Returns("Anything");
            controllerDescriptorMock.Setup(cd => cd.ControllerType).Returns("Anything".GetType());
            var actionDescriptorMock = new Mock<ActionDescriptor>();
            actionDescriptorMock.Setup(ad => ad.ControllerDescriptor).Returns(controllerDescriptorMock.Object);
            actionDescriptorMock.Setup(ad => ad.ActionName).Returns("Index");

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.Proxy).Returns(new ActionInvokerState
            {
                Offset = 10,
                Arguments = new ActionInvoker.InvokeActionMethod.Arguments(new object[]
                                                                                                                                                            {
                                                                                                                                                                new ControllerContext(),
                                                                                                                                                                actionDescriptorMock.Object,
                                                                                                                                                                new Dictionary<string, object>()
                                                                                                                                                            })});
            contextMock.Setup(c => c.ReturnValue).Returns(new ContentResult());
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimeStrategy);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStategy);

            var implementation = new AsyncActionInvoker.EndInvokeActionMethod();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            brokerMock.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()));
            brokerMock.Verify(b => b.Publish(It.IsAny<TimerResultMessage>()));

        }
    }
}