using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class DependencyResolverShould
    {
        [Theory, AutoMock]
        public void ReturnAllMethods(DependencyResolver sut)
        {
            Assert.NotEmpty(sut.AllMethods);
        }
    }
}
