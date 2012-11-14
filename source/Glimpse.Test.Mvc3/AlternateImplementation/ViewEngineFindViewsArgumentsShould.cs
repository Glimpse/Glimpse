using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewEngineFindViewsArgumentsShould
    {
        [Theory, AutoMock]
        public void ConstructForPartial(ControllerContext controllerContext, string viewName, bool useCache)
        {
            var sut = new ViewEngine.FindViews.Arguments(true, controllerContext, viewName, useCache, false); // last false is a lie to prove the test

            Assert.Equal(controllerContext, sut.ControllerContext);
            Assert.Equal(viewName, sut.ViewName);
            Assert.Equal(useCache, sut.UseCache);
            Assert.Equal(string.Empty, sut.MasterName);
        }

        [Theory, AutoMock]
        public void ConstructForNonPartial(ControllerContext controllerContext, string viewName, bool useCache)
        {
            var sut = new ViewEngine.FindViews.Arguments(false, controllerContext, viewName, "MasterName", useCache);

            Assert.Equal(controllerContext, sut.ControllerContext);
            Assert.Equal(viewName, sut.ViewName);
            Assert.Equal(useCache, sut.UseCache);
            Assert.Equal("MasterName", sut.MasterName);
        }
    }
}