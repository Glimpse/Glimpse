using System.Web.Routing;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ControllerFactoryCreateControllerArgumentsShould
    {
        [Theory, AutoMock]
        public void Construct(RequestContext requestContext, string controllerName)
        {
            var arguments = new ControllerFactory.CreateController.Arguments(requestContext, controllerName);

            Assert.Equal(requestContext, arguments.RequestContext);
            Assert.Equal(controllerName, arguments.ControllerName);
        } 
    }
}