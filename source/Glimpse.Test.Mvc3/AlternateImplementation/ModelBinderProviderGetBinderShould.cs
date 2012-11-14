using System;
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
        public void ProceedAndWrapResultWithRuntimePolicyOn(ModelBinderProvider.GetBinder sut, IAlternateImplementationContext context, Type arg1)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { arg1 });

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.Verify(mb => mb.ReturnValue);
        }
    }
}