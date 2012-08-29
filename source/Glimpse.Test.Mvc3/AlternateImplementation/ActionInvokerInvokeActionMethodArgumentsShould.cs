using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerInvokeActionMethodArgumentsShould
    {
        [Fact]
        public void ConstructForNonAsync()
        {
            var expectedControllerContext = new ControllerContext();
            var expectedActionDescriptor = new Mock<ActionDescriptor>().Object;
            var expectedParameters = new Dictionary<string, object>();

            var arguments = new ActionInvoker.InvokeActionMethod.Arguments(new object[] {expectedControllerContext, expectedActionDescriptor, expectedParameters});

            Assert.Equal(expectedControllerContext, arguments.ControllerContext);
            Assert.Equal(expectedActionDescriptor, arguments.ActionDescriptor);
            Assert.Equal(expectedParameters, arguments.Parameters);
            Assert.False(arguments.IsAsync);
            Assert.Null(arguments.Callback);
            Assert.Null(arguments.State);
        }

        [Fact]
        public void ConstructForAsync()
        {
            var expectedControllerContext = new ControllerContext();
            var expectedActionDescriptor = new Mock<ActionDescriptor>().Object;
            var expectedParameters = new Dictionary<string, object>();
            AsyncCallback expectedCallback = null; //hack for testing!
            var expectedState = "any object";

            var arguments = new ActionInvoker.InvokeActionMethod.Arguments(new object[] { expectedControllerContext, expectedActionDescriptor, expectedParameters, expectedCallback, expectedState });

            Assert.Equal(expectedControllerContext, arguments.ControllerContext);
            Assert.Equal(expectedActionDescriptor, arguments.ActionDescriptor);
            Assert.Equal(expectedParameters, arguments.Parameters);
            Assert.True(arguments.IsAsync);
            Assert.Equal(expectedCallback, arguments.Callback);
            Assert.Equal(expectedState, arguments.State);
        } 
    }
}