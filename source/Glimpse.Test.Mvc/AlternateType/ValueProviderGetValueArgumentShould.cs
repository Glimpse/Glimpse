using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ValidatedValueProviderGetValueArgumentShould : ValueProviderGetValueArgumentShould<IValueProvider>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class UnvalidatedValueProviderGetValueArgumentShould : ValueProviderGetValueArgumentShould<IUnvalidatedValueProvider>
    {
    }

    public abstract class ValueProviderGetValueArgumentShould<T> where T : class
    {
        [Theory, AutoMock]
        public void ConstructWithTwoArguments(string key, bool skipValidation)
        {
            var sut = new ValueProvider<T>.GetValue.Arguments(key, skipValidation);

            Assert.Equal(key, sut.Key);
            Assert.Equal(skipValidation, sut.SkipValidation);
        }

        [Theory, AutoMock]
        public void ConstructWithOneArguments(string key)
        {
            var sut = new ValueProvider<T>.GetValue.Arguments(key);

            Assert.Equal(key, sut.Key);
            Assert.False(sut.SkipValidation);
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidKey(object key, bool skipValidation)
        {
            Assert.Throws<InvalidCastException>(() => new ValueProvider<T>.GetValue.Arguments(key, skipValidation));
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidSkipValidation(string key, object skipValidation)
        {
            Assert.Throws<InvalidCastException>(() => new ValueProvider<T>.GetValue.Arguments(key, skipValidation));
        }
    }
}