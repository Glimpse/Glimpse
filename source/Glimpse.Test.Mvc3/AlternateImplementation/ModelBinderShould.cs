using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ModelBinderShould
    {
        [Theory, AutoMock]
        public void Constuct(IProxyFactory proxyFactory)
        {
            var sut = new ModelBinder(proxyFactory);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<Alternate<DefaultModelBinder>>(sut);
        }

        [Theory, AutoMock]
        public void ImplementTwoMethods(ModelBinder sut)
        {
            Assert.Equal(2, sut.AllMethods().Count());
        }
    }
}