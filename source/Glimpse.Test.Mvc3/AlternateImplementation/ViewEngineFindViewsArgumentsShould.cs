using System.Web.Mvc;
using Glimpse.Mvc3.AlternateImplementation;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewEngineFindViewsArgumentsShould
    {
        [Fact]
        public void ConstructForPartial()
        {
            var controllerContext = new ControllerContext();
            var viewName = "anything";
            var useCache = true;

            var args = new object[] { controllerContext, viewName, useCache, false }; //last false is a lie to prove the test

            var arguments = new ViewEngine.FindViews.Arguments(args, true);

            Assert.Equal(controllerContext, arguments.ControllerContext);
            Assert.Equal(viewName, arguments.ViewName);
            Assert.Equal(useCache, arguments.UseCache);
            Assert.Equal(string.Empty, arguments.MasterName);
        }

        [Fact]
        public void ConstructForNonPartial()
        {
            var controllerContext = new ControllerContext();
            var viewName = "anything";
            var useCache = true;

            var args = new object[] { controllerContext, viewName, "MasterName", useCache }; 

            var arguments = new ViewEngine.FindViews.Arguments(args, false);

            Assert.Equal(controllerContext, arguments.ControllerContext);
            Assert.Equal(viewName, arguments.ViewName);
            Assert.Equal(useCache, arguments.UseCache);
            Assert.Equal("MasterName", arguments.MasterName);
        }
    }
}