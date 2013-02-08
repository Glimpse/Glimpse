using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Message
{
    public class ActionMessageShould
    {
        [Theory(Skip = "Ned to get AutoFixture Working."), AutoMock]
        public void ShouldBeAbleToBuildWithFactoryUsingActionDescriptor(ActionDescriptor descriptor)
        { 
            var testMessage = new TestMessage().AsActionMessage(descriptor);

            Assert.Equal(descriptor.ActionName, testMessage.ActionName);
            Assert.Equal(descriptor.ControllerDescriptor.ControllerName, testMessage.ControllerName);
        }

        [Theory(Skip = "Ned to get AutoFixture Working."), AutoMock]
        public void ShouldBeAbleToBuildWithFactoryUsingControllerContext(ControllerContext controllerContext, string controllerName, string actionName)
        {
            controllerContext.Controller.ValueProvider.Setup(x => x.GetValue("controller")).Returns(new ValueProviderResult(controllerName, null, null));
            controllerContext.Controller.ValueProvider.Setup(x => x.GetValue("name")).Returns(new ValueProviderResult(actionName, null, null));

            var testMessage = new TestMessage().AsActionMessage(controllerContext);

            Assert.Equal(controllerName, testMessage.ControllerName);
            Assert.Equal(actionName, testMessage.ActionName);
        }

        public class TestMessage : IActionMessage
        {
            public Guid Id { get; private set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
        }
    }
}
