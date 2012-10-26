using System.Linq;
using Glimpse.Core.Extensibility;
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
            var implementations = new View(new Mock<IProxyFactory>().Object).AllMethods();

            Assert.True(implementations.Count() == 1);
        }
    }
}