using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AsyncActionInvokerBeginInvokerActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            IAlternateImplementation<AsyncControllerActionInvoker> implementation = new AsyncActionInvoker.BeginInvokeActionMethod();

            Assert.Equal("BeginInvokeActionMethod", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.Off;

            var implementation = new AsyncActionInvoker.BeginInvokeActionMethod();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimePolicyStrategy);
            contextMock.Setup(c => c.TimerStrategy).Returns(() => new Mock<IExecutionTimer>().Object);
            contextMock.Setup(c => c.MessageBroker).Returns(new Mock<IMessageBroker>().Object);

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.Proxy, Times.Never());
        }

        [Fact]
        public void StartTimingExecution()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;

            var implementation = new AsyncActionInvoker.BeginInvokeActionMethod();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(runtimePolicyStrategy);
            contextMock.Setup(c => c.TimerStrategy).Returns(() => new Mock<IExecutionTimer>().Object);
            contextMock.Setup(c => c.MessageBroker).Returns(new Mock<IMessageBroker>().Object);
            var stateMock = new Mock<IActionInvokerState>();
            contextMock.Setup(c => c.Proxy).Returns(stateMock.Object);
            contextMock.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext(),
                                                                new Mock<ActionDescriptor>().Object,
                                                                new Dictionary<string,object>(),
                                                                new AsyncCallback(delegate {  }),
                                                                "state",
                                                            });

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c => c.Proceed());
            contextMock.Verify(c=>c.Proxy);
            stateMock.VerifySet(s=>s.Offset = It.IsAny<long>());
        }
    }
}