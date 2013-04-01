using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.AlternateType
{
    public class ModelBinderProviderShould
    {
        [Theory, AutoMock]
        public void Construct(IProxyFactory proxyFactory)
        {
            AlternateType<IModelBinderProvider> sut = new ModelBinderProvider(proxyFactory);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<AlternateType<IModelBinderProvider>>(sut);
        }

        [Theory, AutoMock]
        public void ImplementOneMethod(ModelBinderProvider sut)
        {
            Assert.Equal(1, sut.AllMethods.Count());
        }
    }
}