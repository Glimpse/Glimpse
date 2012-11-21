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
            var sut = new AsyncActionInvoker(new Mock<IProxyFactory>().Object).AllMethods();

            Assert.NotEmpty(sut);
        }
    }
}