using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class FactoryShould
    {
        [Fact]
        public void ConstructWithProviderServiceLocator()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(serviceLocatorMock.Object);
            Assert.NotNull(factory);
            Assert.Equal(serviceLocatorMock.Object, factory.ProviderServiceLocator);
        }

        [Fact]
        public void ConstructWithBothServiceLocators()
        {
            var userLocatorMock = new Mock<IServiceLocator>();
            var providerLocatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(providerLocatorMock.Object, userLocatorMock.Object);
            Assert.NotNull(factory);
            Assert.Equal(providerLocatorMock.Object, factory.ProviderServiceLocator);
            Assert.Equal(userLocatorMock.Object, factory.UserServiceLocator);
        }

        [Fact]
        public void ConstructWithoutServiceLocator()
        {
            var factory = new Factory();
            Assert.NotNull(factory);
            Assert.Null(factory.UserServiceLocator);
            Assert.Null(factory.ProviderServiceLocator);
        }

        [Fact]
        public void InstantiateRuntimeLeveragesIServiceLocator()
        {
            var runtimeMock = new Mock<IGlimpseRuntime>();
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(sl => sl.GetInstance<IGlimpseRuntime>()).Returns(runtimeMock.Object);

            var factory = new Factory(serviceLocatorMock.Object);
            var result = factory.InstantiateRuntime();
            Assert.Equal(runtimeMock.Object, result);
        }

        [Fact]
        public void InstantiateRuntimeReturnsDefaultInstance()
        {
            var factory = new Factory();
            Assert.Throws<NotImplementedException>(() => factory.InstantiateRuntime());
        }

        [Fact]
        public void InstantiateFrameworkProviderLeveragesIServiceLocator()
        {
            var providerMock = new Mock<IFrameworkProvider>();
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(sl => sl.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);

            var factory = new Factory(serviceLocatorMock.Object);
            var result = factory.InstantiateFrameworkProvider();
            Assert.Equal(providerMock.Object, result);
        }

        [Fact]
        public void InstantiateFrameworkProviderWithoutIServiceLocator()
        {
            var factory = new Factory();
            Assert.Throws<GlimpseException>(()=>factory.InstantiateFrameworkProvider());
        }

        [Fact]
        public void InstantiateResourceEndpointConfigLeveragesIServiceLocator()
        {
            var endpointConfigMock = new Mock<ResourceEndpointConfiguration>();
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(sl => sl.GetInstance<ResourceEndpointConfiguration>()).Returns(endpointConfigMock.Object);

            var factory = new Factory(serviceLocatorMock.Object);
            var result = factory.InstantiateEndpointConfiguration();
            Assert.Equal(endpointConfigMock.Object, result);
        }

        [Fact]
        public void InstantiateResourceEndpointConfigWithoutIServiceLocator()
        {
            var factory = new Factory();
            Assert.Throws<GlimpseException>(() => factory.InstantiateEndpointConfiguration());
        }

        [Fact]
        public void InstantiateClientScriptsLeveragesIServiceLocator()
        {
            var clientScripts = new List<IClientScript>();
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(sl => sl.GetInstance<IList<IClientScript>>()).Returns(clientScripts);

            var factory = new Factory(serviceLocatorMock.Object);
            var result = factory.InstantiateClientScripts();
            Assert.Equal(clientScripts, result);
        }

        [Fact]
        public void InstantiateClientScriptsWithoutIServiceLocator()
        {
            var factory = new Factory();
            var result = factory.InstantiateClientScripts();
            Assert.False(result.Any());
        }

    }
}