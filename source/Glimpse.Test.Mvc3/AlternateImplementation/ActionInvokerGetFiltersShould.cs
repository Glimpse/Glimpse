using System;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerGetFiltersShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            Func<RuntimePolicy> policyStrategy = ()=>RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();
            var factoryMock = new Mock<IProxyFactory>();

            var implementation = new ActionInvoker.GetFilters<ControllerActionInvoker>(policyStrategy, timerStrategy, brokerMock.Object, factoryMock.Object);

            Assert.Equal("GetFilters", implementation.MethodToImplement.Name);

            var asyncImplementation = new ActionInvoker.GetFilters<AsyncControllerActionInvoker>(policyStrategy, timerStrategy, brokerMock.Object, factoryMock.Object);

            Assert.Equal("GetFilters", asyncImplementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProceedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();
            var factoryMock = new Mock<IProxyFactory>();


            var contextMock = new Mock<IAlternateImplementationContext>();

            var implementation = new ActionInvoker.GetFilters<ControllerActionInvoker>(policyStrategy, timerStrategy, brokerMock.Object, factoryMock.Object);

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

            var implementation = new ActionInvoker.GetFilters<ControllerActionInvoker>(policyStrategy, timerStrategy, brokerMock.Object, factoryMock.Object);

            implementation.NewImplementation(contextMock.Object);

            contextMock.Verify(c=>c.Proceed());
            contextMock.Verify(c => c.ReturnValue);
            factoryMock.Verify(f=>f.IsProxyable(It.IsAny<object>()), Times.AtLeastOnce());
        }
    }
}