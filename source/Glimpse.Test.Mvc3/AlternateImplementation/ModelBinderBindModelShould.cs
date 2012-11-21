using System;
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
    public class ModelBinderBindModelShould
    {
        [Theory, AutoMock]
        public void ImplementProperMethod(ModelBinder.BindModel sut)
        {
            Assert.Equal("BindModel", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ModelBinder.BindModel sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedPublishMessageWithRuntimePolicyOn(ModelBinder.BindModel sut, IAlternateImplementationContext context, ControllerContext arg1, ModelBindingContext arg2)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1, arg2 });

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ModelBinder.BindModel.Message>()));
        }
    }
}