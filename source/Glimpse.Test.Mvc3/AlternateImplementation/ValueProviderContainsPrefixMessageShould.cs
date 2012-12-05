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
    public class ValidatedValueProviderContainsPrefixMessageShould : ValueProviderContainsPrefixMessageShould<IValueProvider>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class UnvalidatedValueProviderContainsPrefixMessageShould : ValueProviderContainsPrefixMessageShould<IUnvalidatedValueProvider>
    {
    }

    public abstract class ValueProviderContainsPrefixMessageShould<T> where T : class
    {
        [Theory, AutoMock]
        public void Construct(string prefix, bool containsPrefix, Type valueProviderType, MethodInfo executedMethod)
        {
            var sut = new ValueProvider<T>.ContainsPrefix.Message(prefix, containsPrefix, valueProviderType, executedMethod);

            Assert.Equal(prefix, sut.Prefix);
            Assert.Equal(containsPrefix, sut.IsMatch);
            Assert.Equal(valueProviderType, sut.ValueProviderType);
        }

        [Theory, AutoMock]
        public void SetId(ValueProvider<T>.ContainsPrefix.Message sut)
        {
            Assert.NotEqual(default(Guid), sut.Id);
        }
    }
}