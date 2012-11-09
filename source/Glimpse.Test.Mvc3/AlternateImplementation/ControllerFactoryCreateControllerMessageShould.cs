using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerMessageShould
    {
        [Theory, AutoMock]
        public void Construct(RequestContext requestContext, string controllerName, IController controller)
        {
            var arguments = new ControllerFactory.CreateController.Arguments(new object[] { requestContext, controllerName });

            var sut = new ControllerFactory.CreateController.Message(arguments, controller);

            Assert.Equal(requestContext.RouteData, sut.RouteData);
            Assert.Equal(controllerName, sut.ControllerName);
            Assert.True(sut.IsControllerResolved);
            Assert.Equal(controller.GetType(), sut.ControllerType);
        }

        [Theory, AutoMock]
        public void ConstructWithNullController(RequestContext requestContext, string controllerName, IController controller)
        {
            var arguments = new ControllerFactory.CreateController.Arguments(new object[] { requestContext, controllerName });

            var sut = new ControllerFactory.CreateController.Message(arguments, null);

            Assert.Equal(requestContext.RouteData, sut.RouteData);
            Assert.Equal(controllerName, sut.ControllerName);
            Assert.False(sut.IsControllerResolved);
            Assert.Null(sut.ControllerType);
        }
    }
}