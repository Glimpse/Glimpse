using System;
using System.ComponentModel;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ModelBinderBindPropertyShould
    {
        [Theory, AutoMock]
        public void ImplementProperMethod(ModelBinder.BindProperty sut)
        {
            Assert.Equal("BindProperty", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ModelBinder.BindProperty sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedAndPublishMessageWithRuntimePolicyOn(ModelBinder.BindProperty sut, IAlternateImplementationContext context, ControllerContext controllerContext, ModelBindingContext modelBindingContext, PropertyDescriptor propertyDescriptor)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { controllerContext, modelBindingContext, propertyDescriptor });

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ModelBinder.BindProperty.Message>()));
        }
    }
}