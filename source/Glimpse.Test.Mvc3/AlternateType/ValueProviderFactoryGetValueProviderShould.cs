using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using ValueProviderFactory = Glimpse.Mvc.AlternateType.ValueProviderFactory;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ValueProviderFactoryGetValueProviderShould
    {
        [Theory, AutoMock]
        public void ImplementProperMethod(ValueProviderFactory.GetValueProvider sut)
        {
            Assert.Equal("GetValueProvider", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ValueProviderFactory.GetValueProvider sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithNullReturnValue(ValueProviderFactory.GetValueProvider sut, IAlternateMethodContext context, ControllerContext arg1)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(null);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);            
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithIUnvalidatedValueProviderReturnValue([Frozen] IProxyFactory proxyFactory, ValueProviderFactory.GetValueProvider sut, IAlternateMethodContext context, ControllerContext arg1, IUnvalidatedValueProvider returnValue)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);
            proxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IUnvalidatedValueProvider>(typeof(IUnvalidatedValueProvider))).Returns(true);
            proxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IUnvalidatedValueProvider>(), It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(returnValue);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);
            context.VerifySet(c => c.ReturnValue = It.IsAny<IUnvalidatedValueProvider>());
        }

        [Theory, AutoMock]
        public void ProceedWithTimerWithIValueProviderReturnValue([Frozen] IProxyFactory proxyFactory, ValueProviderFactory.GetValueProvider sut, IAlternateMethodContext context, ControllerContext arg1, IValueProvider returnValue)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });
            context.Setup(c => c.ReturnValue).Returns(returnValue);
            proxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IValueProvider>(typeof(IValueProvider))).Returns(true);
            proxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IValueProvider>(), It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(returnValue);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(c => c.Time(It.IsAny<Action>()));
            context.Verify(c => c.ReturnValue);
            context.VerifySet(c => c.ReturnValue = It.IsAny<IValueProvider>());
        }
    }
}