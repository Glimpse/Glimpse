using System.Web.Mvc;
using System.Web.Routing;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ControllerFactoryCreateControllerMessageShould
    {
        [Fact]
        public void Construct()
        {

            var routeData = new RouteData();
            var requestContext = new RequestContext{RouteData = routeData};
            var controllerName = "aName";
            var arguments = new ControllerFactory.CreateController.Arguments(new object[] { requestContext, controllerName });

            var controllerMock = new Mock<IController>();

            var message = new ControllerFactory.CreateController.Message(arguments, controllerMock.Object);

            Assert.Equal(routeData, message.RouteData);
            Assert.Equal(controllerName, message.ControllerName);
            Assert.True(message.IsControllerResolved);
            Assert.Equal(controllerMock.Object.GetType(), message.ControllerType);
        }

        [Fact]
        public void ConstructWithNullController()
        {

            var routeData = new RouteData();
            var requestContext = new RequestContext { RouteData = routeData };
            var controllerName = "aName";
            var arguments = new ControllerFactory.CreateController.Arguments(new object[] { requestContext, controllerName });

            var message = new ControllerFactory.CreateController.Message(arguments, null);

            Assert.Equal(routeData, message.RouteData);
            Assert.Equal(controllerName, message.ControllerName);
            Assert.False(message.IsControllerResolved);
            Assert.Null(message.ControllerType);
        }
    }
}