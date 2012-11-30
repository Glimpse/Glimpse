using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryShould
    {
        [Theory, AutoMock]
        public void ReturnAllAlternateImplementations(ControllerFactory sut)
        {
            Assert.NotEmpty(sut.AllMethods);
        }
    }
}