using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverShould
    {
        [Fact]
        public void ReturnAllMethods()
        {
            var allMethods = new DependencyResolver(new Mock<IProxyFactory>().Object).AllMethods();

            Assert.NotEmpty(allMethods);
        }
    }
}
