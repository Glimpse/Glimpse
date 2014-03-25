using System.Linq;
using Glimpse.AspNet.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.AlternateType
{
    public class RouteShould
    {
        [Theory, AutoMock]
        public void ReturnOneMethod(IProxyFactory proxyFactory, ILogger logger)
        {
            var alternationImplementation = new Route(proxyFactory, logger);

            Assert.Equal(2, alternationImplementation.AllMethodsRouteBase.Count());
            Assert.Equal(3, alternationImplementation.AllMethodsRoute.Count());
        } 
    }
}
