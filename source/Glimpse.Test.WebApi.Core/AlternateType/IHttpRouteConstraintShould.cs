using System.Linq;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.WebApi.Core.AlternateType
{
    public class IHttpRouteConstraintShould
    {
        [Theory, AutoMock]
        public void ReturnOneMethod(IProxyFactory proxyFactory)
        {
            AlternateType<System.Web.Http.Routing.IHttpRouteConstraint> alternationImplementation = new IHttpRouteConstraint(proxyFactory);

            Assert.Equal(1, alternationImplementation.AllMethods.Count());
        }

        [Theory, AutoMock]
        public void SetProxyFactory(IProxyFactory proxyFactory)
        {
            AlternateType<System.Web.Http.Routing.IHttpRouteConstraint> alternationImplementation = new IHttpRouteConstraint(proxyFactory);

            Assert.Equal(proxyFactory, alternationImplementation.ProxyFactory);
        }
    }
}
