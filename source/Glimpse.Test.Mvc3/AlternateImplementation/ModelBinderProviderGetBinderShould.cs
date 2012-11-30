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
    public class ModelBinderProviderGetBinderShould
    {
        [Theory, AutoMock]
        public void Construct(AlternateType<DefaultModelBinder> alternateModelBinder)
        {
            var sut = new ModelBinderProvider.GetBinder(alternateModelBinder);

            Assert.Equal(alternateModelBinder, sut.AlternateModelBinder);
            Assert.NotNull(sut.MethodToImplement);
        }

        [Theory, AutoMock]
        public void ImplementProperMethod(ModelBinderProvider.GetBinder sut)
        {
            Assert.Equal("GetBinder", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ModelBinderProvider.GetBinder sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedAndWrapResultWithRuntimePolicyOnAndDefaultModelBinder(AlternateType<DefaultModelBinder> alternateModelBinder, IAlternateImplementationContext context, Type arg1, DefaultModelBinder returnValue, DefaultModelBinder newModelBinder)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);
            alternateModelBinder.Setup(amb => amb.TryCreate(It.IsAny<DefaultModelBinder>(), out newModelBinder, null, null)).Returns(true);

            var sut = new ModelBinderProvider.GetBinder(alternateModelBinder);
            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.Verify(mb => mb.ReturnValue);
            context.Logger.Verify(l => l.Warn(It.IsAny<string>(), context.ReturnValue.GetType()), Times.Never());
            context.VerifySet(c => c.ReturnValue = newModelBinder);
            alternateModelBinder.Verify(amb => amb.TryCreate(It.IsAny<DefaultModelBinder>(), out newModelBinder, null, null));
        }

        [Theory, AutoMock]
        public void ProceedAndWarnWithRuntimePolicyOnAndIModelBinder(AlternateType<DefaultModelBinder> alternateModelBinder, IAlternateImplementationContext context, Type arg1, IModelBinder returnValue)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);

            var sut = new ModelBinderProvider.GetBinder(alternateModelBinder);
            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.Verify(mb => mb.ReturnValue);
            context.Logger.Verify(l => l.Warn(It.IsAny<string>(), context.ReturnValue.GetType()));
        }
    }
}