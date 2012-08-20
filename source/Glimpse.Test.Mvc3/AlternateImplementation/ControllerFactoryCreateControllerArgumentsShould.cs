using System.Web.Routing;
using Glimpse.Mvc3.AlternateImplementation;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerArgumentsShould
    {
        [Fact]
        public void Construct()
        {
            var requestContext = new RequestContext();
            var controllerName = "aName";
            var arguments = new ControllerFactory.CreateController.Arguments(new object []{requestContext, controllerName});

            Assert.Equal(requestContext, arguments.RequestContext);
            Assert.Equal(controllerName, arguments.ControllerName);
        } 
    }
}