using System.Linq;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.AlternateImplementation
{
    public class RouteShould
    {
        [Theory, AutoMock]
        public void ReturnTwoMethods(IProxyFactory proxyFactory)
        {
            Alternate<System.Web.Routing.Route> alternationImplementation = new Route(proxyFactory);

            Assert.Equal(2, alternationImplementation.AllMethods().Count());
        }

        [Theory, AutoMock]
        public void SetProxyFactory(IProxyFactory proxyFactory)
        {
            Alternate<System.Web.Routing.Route> alternationImplementation = new Route(proxyFactory);

            Assert.Equal(proxyFactory, alternationImplementation.ProxyFactory);
        }
    }
}
