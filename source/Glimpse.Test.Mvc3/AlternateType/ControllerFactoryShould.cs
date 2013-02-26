using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ControllerFactoryShould
    {
        [Theory, AutoMock]
        public void ReturnAllAlternateMethods(ControllerFactory sut)
        {
            Assert.NotEmpty(sut.AllMethods);
        }
    }
}