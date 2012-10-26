using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryShould
    {
        [Fact]
        public void ReturnAllAlternateImplementations()
        {
            var implementations = new ControllerFactory(new Mock<IProxyFactory>().Object).AllMethods();

            Assert.NotEmpty(implementations);
        }
    }
}