using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class ValidatedValueProviderShould : ValueProviderShould<IValueProvider>
    {
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it only changes the generic T parameter for the abstract class below.")]
    public class UnvalidatedValueProviderShould : ValueProviderShould<IUnvalidatedValueProvider>
    {
    }

    public abstract class ValueProviderShould<T> where T : class
    {
        [Theory, AutoMock]
        public void Construct(IProxyFactory proxyFactory)
        {
            AlternateType<T> sut = new ValueProvider<T>(proxyFactory);

            Assert.IsAssignableFrom<AlternateType<T>>(sut);
            Assert.NotNull(sut);
        }

        [Theory, AutoMock]
        public void ImplementOneMethod(ValueProvider<T> sut)
        {
            Assert.Equal(2, sut.AllMethods().Count());
        }
    }
}