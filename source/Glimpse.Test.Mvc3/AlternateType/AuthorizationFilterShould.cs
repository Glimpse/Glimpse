using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class AuthorizationFilterShould
    {
        [Theory, AutoMock]
        public void SetProxyFactory(IProxyFactory proxyFactory)
        {
            AlternateType<IAuthorizationFilter> sut = new AuthorizationFilter(proxyFactory);

            Assert.Equal(proxyFactory, sut.ProxyFactory);
        }

        [Theory, AutoMock]
        public void ReturnOneMethod(IProxyFactory proxyFactory)
        {
            AlternateType<IAuthorizationFilter> sut = new AuthorizationFilter(proxyFactory);

            Assert.Equal(1, sut.AllMethods.Count());
        } 
    }
}