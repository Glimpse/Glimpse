using System;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using ValueProviderFactory = Glimpse.Mvc.AlternateImplementation.ValueProviderFactory;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ValueProviderFactoryGetValueProviderShould
    {
        [Theory, AutoMock]
        public void Construct(Alternate<IValueProvider> alternateValueProvider, Alternate<IUnvalidatedValueProvider> alternateUnvalidatedValueProvider)
        {
            var sut = new ValueProviderFactory.GetValueProvider(alternateValueProvider, alternateUnvalidatedValueProvider);

            Assert.Equal(alternateValueProvider, sut.AlternateValidatedValueProvider);
            Assert.Equal(alternateUnvalidatedValueProvider, sut.AlternateUnvalidatedValueProvider);
            Assert.NotNull(sut.MethodToImplement);
        }

        [Theory, AutoMock]
        public void ImplementProperMethod(ValueProviderFactory.GetValueProvider sut)
        {
            Assert.Equal("GetValueProvider", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ValueProviderFactory.GetValueProvider sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithNullReturnValue(ValueProviderFactory.GetValueProvider sut, IAlternateImplementationContext context, ControllerContext arg1)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(null);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);            
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithIUnvalidatedValueProviderReturnValue([Frozen] Alternate<IUnvalidatedValueProvider> alternateUnvalidatedValueProvider, ValueProviderFactory.GetValueProvider sut, IAlternateImplementationContext context, ControllerContext arg1, IUnvalidatedValueProvider returnValue)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);
            alternateUnvalidatedValueProvider.Setup(a => a.TryCreate(It.IsAny<IUnvalidatedValueProvider>(), out returnValue)).Returns(true);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);
            context.VerifySet(c => c.ReturnValue = returnValue);
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithIValueProviderReturnValue([Frozen] Alternate<IValueProvider> alternateValueProvider, ValueProviderFactory.GetValueProvider sut, IAlternateImplementationContext context, ControllerContext arg1, IValueProvider returnValue)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);
            alternateValueProvider.Setup(a => a.TryCreate(It.IsAny<IValueProvider>(), out returnValue)).Returns(true);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);
            context.VerifySet(c => c.ReturnValue = returnValue);
        }
    }
}