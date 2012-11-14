using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ModelBinderProviderShould
    {
        [Theory, AutoMock]
        public void Construct(IProxyFactory proxyFactory)
        {
            Alternate<IModelBinderProvider> sut = new ModelBinderProvider(proxyFactory);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<Alternate<IModelBinderProvider>>(sut);
        }

        [Theory, AutoMock]
        public void ImplementOneMethod(ModelBinderProvider sut)
        {
            Assert.Equal(1, sut.AllMethods().Count());
        }
    }
}