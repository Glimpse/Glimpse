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
            IAlternateImplementation<AsyncControllerActionInvoker> implementation = new AsyncActionInvoker.BeginInvokeActionMethod(()=>RuntimePolicy.On, ()=>new Mock<IExecutionTimer>().Object, new Mock<IMessageBroker>().Object);

            Assert.Equal("BeginInvokeActionMethod", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ConstructWithRuntimePolicyStrategy()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = ()=>RuntimePolicy.On;

            var implementation = new AsyncActionInvoker.BeginInvokeActionMethod(runtimePolicyStrategy, () => new Mock<IExecutionTimer>().Object, new Mock<IMessageBroker>().Object);

            Assert.Equal(runtimePolicyStrategy, implementation.RuntimePolicyStrategy);
        }

        [Fact]
        public void ProceedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.Off;

            var implementation = new AsyncActionInvoker.BeginInvokeActionMethod(runtimePolicyStrategy, () => new Mock<IExecutionTimer>().Object, new Mock<IMessageBroker>().Object);

            var contextMock = new Mock<IAlternateImplementationContext>();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.Proxy, Times.Never());
        }

        [Fact]
        public void StartTimingExecution()
        {
            Func<RuntimePolicy> runtimePolicyStrategy = () => RuntimePolicy.On;

            var implementation = new AsyncActionInvoker.BeginInvokeActionMethod(runtimePolicyStrategy, () => new Mock<IExecutionTimer>().Object, new Mock<IMessageBroker>().Object);

            var contextMock = new Mock<IAlternateImplementationContext>();
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