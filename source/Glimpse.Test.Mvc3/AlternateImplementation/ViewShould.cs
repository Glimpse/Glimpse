using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewShould
    {
        [Fact]
        public void GetAlternateImplementations()
        {
            var brokerMock = new Mock<IMessageBroker>();
            Func<IExecutionTimer> timer = () => new Mock<IExecutionTimer>().Object;
            Func<RuntimePolicy> runtime = () => RuntimePolicy.On;

            var implementations = View.AllMethods(brokerMock.Object, timer, runtime);

            Assert.True(implementations.Count() == 1);
        }
    }
}