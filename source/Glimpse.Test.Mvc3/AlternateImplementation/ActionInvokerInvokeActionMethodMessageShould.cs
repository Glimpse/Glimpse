using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerInvokeActionMethodMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ActionDescriptor actionDescriptor, MethodInfo method, TimerResult timer, ActionResult result)
        {
            var expectedControllerType = typeof(Controller);
            actionDescriptor.ControllerDescriptor.Setup(cd => cd.ControllerType).Returns(expectedControllerType);
            var arguments = new ActionInvoker.InvokeActionMethod.Arguments(new object[]
                                                                 {
                                                                     new ControllerContext(),
                                                                     actionDescriptor,
                                                                     new Dictionary<string, object>()
                                                                 });

            var sut = new ActionInvoker.InvokeActionMethod.Message(arguments, result, method, timer);

            Assert.Equal(actionDescriptor.ControllerDescriptor.ControllerName, sut.ControllerName);
            Assert.Equal(expectedControllerType, sut.ExecutedType);
            Assert.Equal(actionDescriptor.ActionName, sut.ActionName);
            Assert.False(sut.IsChildAction);
            Assert.Equal(result.GetType(), sut.ActionResultType);
        }
    }
}