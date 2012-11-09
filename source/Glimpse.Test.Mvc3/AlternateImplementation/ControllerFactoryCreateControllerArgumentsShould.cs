using System.Web.Routing;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerArgumentsShould
    {
        [Theory, AutoMock]
        public void Construct(RequestContext requestContext, string controllerName)
        {
            var arguments = new ControllerFactory.CreateController.Arguments(new object[] { requestContext, controllerName });

            Assert.Equal(requestContext, arguments.RequestContext);
            Assert.Equal(controllerName, arguments.ControllerName);
        } 
    }
}