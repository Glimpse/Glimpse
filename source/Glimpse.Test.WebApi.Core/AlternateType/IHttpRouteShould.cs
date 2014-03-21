using System.Linq;
using Glimpse.WebApi.Core.AlternateType;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.WebApi.Core.AlternateType
{
    public class IHttpRouteShould
    {
        [Theory, AutoMock]
        public void ReturnOneMethod(IProxyFactory proxyFactory, ILogger logger)
        {
            var alternationImplementation = new IHttpRoute(proxyFactory, logger);

            Assert.Equal(2, alternationImplementation.AllMethodsIHttpRoute.Count());
            Assert.Equal(3, alternationImplementation.AllMethodsHttpRoute.Count());
        } 
    }
}
