using System.Diagnostics;
using System.Linq;
using System;
using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class CastleDynamicProxyFactoryShould
    {
        [Fact]
        public void ConstructWithLogger()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(loggerMock.Object, factory.Logger);
            Assert.NotNull(factory.ProxyGenerator);
        }

        [Fact]
        public void ThrowsWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(null, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
        }

        [Fact]
        public void CreateProxies()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

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

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.False(factory.IsProxyable("any string"));
        }

        [Fact]
        public void ReturnTrueForOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            var dummyTab = new DummyTab();

            Assert.True(factory.IsProxyable(dummyTab));
        }

        [Fact]
        public void ReturnFalseForAlreadyProxiedObjectsOnIsProxyable()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

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
            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

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