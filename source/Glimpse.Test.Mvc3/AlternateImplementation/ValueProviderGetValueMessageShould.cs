using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ValidatedValueProviderGetValueMessageShould : ValueProviderGetValueMessageShould<IValueProvider>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class UnvalidatedValueProviderGetValueMessageShould : ValueProviderGetValueMessageShould<IUnvalidatedValueProvider>
    {
    }

    public abstract class ValueProviderGetValueMessageShould<T> where T : class
    {
        [Theory, AutoMock]
        public void Construct(ValueProvider<T>.GetValue.Arguments arguments, ValueProviderResult result, Type valueProviderType, MethodInfo executedMethod)
        {
            var sut = new ValueProvider<T>.GetValue.Message(arguments, result, valueProviderType, executedMethod);

            Assert.Equal(result.AttemptedValue, sut.AttemptedValue);
            Assert.Equal(result.Culture, sut.Culture);
            Assert.Equal(result.RawValue, sut.RawValue);
            Assert.Equal(valueProviderType, sut.ValueProviderType);
        }
    }
}