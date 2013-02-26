using System.Linq;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ViewShould
    {
        [Theory, AutoMock]
        public void GetAlternateMethods(View sut)
        {
            var allMethods = sut.AllMethods;

            Assert.True(allMethods.Count() == 1);
        }
    }
}