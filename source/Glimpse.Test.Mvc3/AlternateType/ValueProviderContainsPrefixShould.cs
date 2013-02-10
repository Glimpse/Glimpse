using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ValidatedValueProviderContainsPrefixShould : ValueProviderContainsPrefixShould<IValueProvider>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class UnvalidatedValueProviderContainsPrefixShould : ValueProviderContainsPrefixShould<IUnvalidatedValueProvider>
    {
    }

    public abstract class ValueProviderContainsPrefixShould<T> where T : class
    {
        [Theory, AutoMock]
        public void ImplementProperMethod(ValueProvider<T>.ContainsPrefix sut)
        {
            Assert.Equal("ContainsPrefix", sut.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedAndReturnWithRuntimePolicyOff(ValueProvider<T>.ContainsPrefix sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<object>()), Times.Never());
        }

        [Theory, AutoMock]
        public void ProceedAndPublishMessageWithRuntimePolicyOn(ValueProvider<T>.ContainsPrefix sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.Arguments).Returns(new object[] { "any sting" });
            context.Setup(c => c.ReturnValue).Returns(true);

            sut.NewImplementation(context);

            context.TimerStrategy().Verify(t => t.Time(It.IsAny<Action>()));
            context.MessageBroker.Verify(mb => mb.Publish(It.IsAny<ValueProvider<T>.ContainsPrefix.Message>()));
        }
    }
}