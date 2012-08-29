using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerInvokeActionMethodMessageShould
    {
        [Fact]
        public void Construct()
        {
            var expectedControllerName = "a name";
            var expectedControllerType = "".GetType();
            var expectedActionName = "an action name";

            var controllerDescriptorMock = new Mock<ControllerDescriptor>();
            controllerDescriptorMock.Setup(cd => cd.ControllerName).Returns(expectedControllerName);
            controllerDescriptorMock.Setup(cd => cd.ControllerType).Returns(expectedControllerType);

            var actionDescriptorMock = new Mock<ActionDescriptor>();
            actionDescriptorMock.Setup(ad => ad.ControllerDescriptor).Returns(controllerDescriptorMock.Object);
            actionDescriptorMock.Setup(ad => ad.ActionName).Returns(expectedActionName);


            var arguments = new ActionInvoker.InvokeActionMethod.Arguments(new object[]
                                                                 {
                                                                     new ControllerContext(),
                                                                     actionDescriptorMock.Object,
                                                                     new Dictionary<string, object>()
                                                                 });
            var returnValue = new ContentResult();

            var message = new ActionInvoker.InvokeActionMethod.Message(arguments, returnValue);

            Assert.Equal(expectedControllerName, message.ControllerName);
            Assert.Equal(expectedControllerType, message.ControllerType);
            Assert.Equal(expectedActionName, message.ActionName);
            Assert.False(message.IsChildAction);
            Assert.Equal(returnValue.GetType(), message.ActionResultType);
        }
    }
}