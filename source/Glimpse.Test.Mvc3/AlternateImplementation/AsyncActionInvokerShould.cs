using System;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class AsyncActionInvokerShould
    {
        [Fact]
        public void ReturnAllMethods()
        {
            Func<RuntimePolicy> policyStrategy = ()=>RuntimePolicy.On;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var implementations = AsyncActionInvoker.AllMethods(policyStrategy, timerStrategy, brokerMock.Object);

            Assert.NotEmpty(implementations);
        }

        [Fact]
        public void ThrowWithNullRuntimePolicyStrategy()
        {
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            Assert.Throws<ArgumentNullException>(() => new AsyncActionInvoker.BeginInvokeActionMethod(null, timerStrategy, brokerMock.Object));
        }

        [Fact]
        public void ThrowWithNullTimerStrategy()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            var brokerMock = new Mock<IMessageBroker>();

            Assert.Throws<ArgumentNullException>(() => new AsyncActionInvoker.BeginInvokeActionMethod(policyStrategy, null, brokerMock.Object));
        }

        [Fact]
        public void ThrowWithNullMessageBroker()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;

            Assert.Throws<ArgumentNullException>(() => new AsyncActionInvoker.BeginInvokeActionMethod(policyStrategy, timerStrategy, null));
        }
    }
}