using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ActionInvokerInvokeActionMethodArgumentsShould
    {
        [Theory, AutoMock]
        public void ConstructForNonAsync(ControllerContext expectedControllerContext, ActionDescriptor expectedActionDescriptor, IDictionary<string, object> expectedParameters)
        {
            var sut = new ActionInvoker.InvokeActionMethod.Arguments(expectedControllerContext, expectedActionDescriptor, expectedParameters);

            Assert.Equal(expectedControllerContext, sut.ControllerContext);
            Assert.Equal(expectedActionDescriptor, sut.ActionDescriptor);
            Assert.Equal(expectedParameters, sut.Parameters);
            Assert.False(sut.IsAsync);
            Assert.Null(sut.Callback);
            Assert.Null(sut.State);
        }

        [Theory, AutoMock]
        public void ConstructForAsync(ControllerContext expectedControllerContext, ActionDescriptor expectedActionDescriptor, IDictionary<string, object> expectedParameters, AsyncCallback expectedCallback, string expectedState)
        {
            var arguments = new ActionInvoker.InvokeActionMethod.Arguments(expectedControllerContext, expectedActionDescriptor, expectedParameters, expectedCallback, expectedState);

            Assert.Equal(expectedControllerContext, arguments.ControllerContext);
            Assert.Equal(expectedActionDescriptor, arguments.ActionDescriptor);
            Assert.Equal(expectedParameters, arguments.Parameters);
            Assert.True(arguments.IsAsync);
            Assert.Equal(expectedCallback, arguments.Callback);
            Assert.Equal(expectedState, arguments.State);
        } 
    }
}