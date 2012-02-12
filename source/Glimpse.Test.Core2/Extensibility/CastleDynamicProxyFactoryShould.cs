using System;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class CastleDynamicProxyFactoryShould
    {
        [Fact]
        public void Construct()
        {
            var loggerMock = new Mock<ILogger>();

            var proxyFactory = new CastleDynamicProxyFactory(loggerMock.Object);

            Assert.Equal(loggerMock.Object, proxyFactory.Logger);
        }

        [Fact]
        public void ThrowWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(()=>new CastleDynamicProxyFactory(null));
        }

        [Fact(Skip = "Come back to me")]
        public void ProxyAnInterface()
        {
            var loggerMock = new Mock<ILogger>();

            var proxyFactory = new CastleDynamicProxyFactory(loggerMock.Object);

            var tabMock = new Mock<ITab>();

            var tabProxy = proxyFactory.CreateProxy(tabMock.Object, Enumerable.Empty<IAlternateMethodImplementation<ITab>>());

            Assert.True(tabProxy.GetType().IsSubclassOf(tabMock.Object.GetType()));
        }
    }
}