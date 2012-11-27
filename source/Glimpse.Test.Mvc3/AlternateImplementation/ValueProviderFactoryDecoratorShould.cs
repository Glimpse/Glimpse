using System;
using System.Web.Mvc;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ValueProviderFactoryDecoratorShould
    {
        [Theory, AutoMock]
        public void Construct(ValueProviderFactory wrappedFactory, Func<IValueProvider, IValueProvider> proxyValueProviderStrategy)
        {
            var sut = new Mvc.AlternateImplementation.ValueProviderFactory.ValueProviderFactoryDecorator(wrappedFactory, proxyValueProviderStrategy);

            Assert.Equal(wrappedFactory, sut.WrappedValueProviderFactory);
            Assert.Equal(proxyValueProviderStrategy, sut.ProxyValueProviderStrategy);
            
            Assert.IsAssignableFrom<ValueProviderFactory>(sut);
        }
    }
}