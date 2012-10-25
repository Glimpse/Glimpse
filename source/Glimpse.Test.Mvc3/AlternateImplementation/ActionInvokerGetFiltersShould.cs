using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerActionInvokerGetFiltersShould : ActionInvokerGetFiltersShould<ControllerActionInvoker>
    {
    }

    public class AsyncControllerActionInvokerGetFiltersShould : ActionInvokerGetFiltersShould<AsyncControllerActionInvoker>
    {
    }

    public abstract class ActionInvokerGetFiltersShould<T> where T : class
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var implementation = new ActionInvoker.GetFilters<T>();

            Assert.Equal("GetFilters", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();
            var factoryMock = new Mock<IProxyFactory>();


            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStrategy);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);

            var implementation = new ActionInvoker.GetFilters<T>();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c=>c.ReturnValue, Times.Never());
        }

        [Fact(Skip = "Need to finish this")]
        public void ProxyFiltersWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();
            var factoryMock = new Mock<IProxyFactory>();

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(brokerMock.Object);
            contextMock.Setup(c => c.ProxyFactory).Returns(factoryMock.Object);
            contextMock.Setup(c => c.TimerStrategy).Returns(timerStrategy);
            contextMock.Setup(c => c.RuntimePolicyStrategy).Returns(policyStrategy);

            var implementation = new ActionInvoker.GetFilters<T>();

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c => c.ReturnValue);
            factoryMock.Verify(f=>f.IsProxyable(It.IsAny<object>()), Times.AtLeastOnce());
        }
    }
}