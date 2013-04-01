using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Message
{ 
    public class ChildActionMessageShould
    {
        [Theory(Skip = "Ned to get AutoFixture Working."), AutoMock]
        public void ShouldBeAbleToBuildWithFactoryUsingActionDescriptor(ControllerBase controller)
        {
            var testMessage = new TestMessage().AsChildActionMessage(controller);

            Assert.Equal(controller.ControllerContext.IsChildAction, testMessage.IsChildAction); 
        } 

        public class TestMessage : IChildActionMessage
        {
            public Guid Id { get; private set; }
            
            public bool IsChildAction { get; set; }

            public string ControllerName { get; set; }

            public string ActionName { get; set; }
        }
    }
}
