using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ActionInvokerInvokeActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            var sut = new ActionInvoker.InvokeActionMethod();

            Assert.Equal("InvokeActionMethod", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProccedAndReturnWithRuntimePolicyOff(ActionInvoker.InvokeActionMethod sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()), Times.Never());
        }

        [Theory, AutoMock]
        public void PublishMessageWithRuntimePolicyOn(ActionInvoker.InvokeActionMethod sut, IAlternateMethodContext context)
        {
            var actionDescriptorMock = new Mock<ActionDescriptor>();
            actionDescriptorMock.Setup(a => a.ControllerDescriptor).Returns(new ReflectedControllerDescriptor(typeof(DummyController)));
            actionDescriptorMock.Setup(a => a.ActionName).Returns("Index");

            context.Setup(c => c.ReturnValue).Returns(new ContentResult());
            context.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext(),
                                                                actionDescriptorMock.Object,
                                                                new Dictionary<string, object>()
                                                            });

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()));
        } 
    }
}