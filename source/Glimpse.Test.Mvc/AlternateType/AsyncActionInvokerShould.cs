using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class AsyncActionInvokerShould
    {
        [Fact]
        public void ReturnAllMethods()
        {
            var sut = new AsyncActionInvoker(new Mock<IProxyFactory>().Object).AllMethods;

            Assert.NotEmpty(sut);
        }
    }
}