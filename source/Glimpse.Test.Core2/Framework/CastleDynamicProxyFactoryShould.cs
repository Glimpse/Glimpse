using System.Linq;
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

        [Fact(Skip = "Need to research this. Seems like we can proxy objects without default constructors.")]
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

        [Fact]
        public void SupportMixins()
        {
            var loggerMock = new Mock<ILogger>();
            var factory = new CastleDynamicProxyFactory(loggerMock.Object);

            string expectedName = "any string";
            var dummyTab = new DummyTab();
            var dummyMixin = new DummyMixin {Name = expectedName};

            var proxy = factory.CreateProxy(dummyTab, Enumerable.Empty<IAlternateImplementation<ITab>>(), dummyMixin);

            Assert.NotNull(proxy);
            Assert.NotNull(proxy as ITab);

            var mixin = proxy as IDummyMixin;

            Assert.NotNull(mixin);
            Assert.Equal(expectedName, mixin.Name);
        }
    }
}