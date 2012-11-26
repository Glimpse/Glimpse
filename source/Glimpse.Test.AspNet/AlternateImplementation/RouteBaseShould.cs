using System.Linq;
using Glimpse.AspNet.AlternateImplementation;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.AlternateImplementation
{
    public class RouteBaseShould
    {
        [Theory, AutoMock]
        public void ReturnOneMethod(IProxyFactory proxyFactory)
        {
            Alternate<System.Web.Routing.RouteBase> alternationImplementation = new RouteBase(proxyFactory);

            Assert.Equal(1, alternationImplementation.AllMethods().Count());
        }

        [Theory, AutoMock]
        public void SetProxyFactory(IProxyFactory proxyFactory)
        {
            Alternate<System.Web.Routing.RouteBase> alternationImplementation = new RouteBase(proxyFactory);

            Assert.Equal(proxyFactory, alternationImplementation.ProxyFactory);
        }
    }
}
