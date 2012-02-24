using System.Linq;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class CastleDynamicProxyFactoryShould
    {
        [Fact]
        public void ConstructWithLogger()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            Assert.Equal(loggerMock.Object, factory.Logger);
            Assert.NotNull(factory.ProxyGenerator);
        }

        [Fact]
        public void ThrowsWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(null));
        }

        [Fact]
        public void CreateProxies()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            var typeMock = new Mock<ITab>();
            var implementationMock = new Mock<IAlternateImplementation<ITab>>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(typeof (ITab).GetMethod("GetData"));
            
            var implementations = new List<IAlternateImplementation<ITab>>
                                      {
                                          implementationMock.Object
                                      };

            var proxy = factory.CreateProxy(typeMock.Object, implementations);

            Assert.NotNull(proxy);
            Assert.NotNull(proxy as ITab);

            proxy.GetData(null);

            implementationMock.Verify(i=>i.NewImplementation(It.IsAny<IAlternateImplementationContext>()));
        }

        [Fact]
        public void ReturnFalseForSealedOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            Assert.False(factory.IsProxyable("any string"));
        }

        [Fact]
        public void ReturnFalseForNoDefaultConstructorOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            Assert.False(factory.IsProxyable(new JsonNetSerializer(loggerMock.Object)));
        }

        [Fact]
        public void ReturnTrueForOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            var dummyTab = new DummyTab();

            Assert.True(factory.IsProxyable(dummyTab));
        }

        [Fact]
        public void ReturnFalseForAlreadyProxiedObjectsOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            var dummyTab = new DummyTab();

            var proxy = factory.CreateProxy(dummyTab, Enumerable.Empty<IAlternateImplementation<ITab>>());

            Assert.NotNull(proxy);
            Assert.NotNull(proxy as ITab);

            var isProxyable = factory.IsProxyable(proxy);

            Assert.False(isProxyable);
        }
    }
}