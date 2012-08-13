using System;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateImplementation
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