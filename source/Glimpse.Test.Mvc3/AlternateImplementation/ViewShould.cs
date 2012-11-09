using System.Linq;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewShould
    {
        [Theory, AutoMock]
        public void GetAlternateImplementations(View sut)
        {
            var allMethods = sut.AllMethods();

            Assert.True(allMethods.Count() == 1);
        }
    }
}