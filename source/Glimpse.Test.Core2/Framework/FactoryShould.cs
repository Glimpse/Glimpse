using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Configuration;
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
            serviceLocatorMock.Setup(sl => sl.GetAllInstances<IClientScript>()).Returns(clientScripts);

            var factory = new Factory(serviceLocatorMock.Object);
            var result = factory.InstantiateClientScripts();


            Assert.Equal(clientScripts, result);
        }

        [Fact]
        public void InstantiateClientScripts()
        {
            var factory = new Factory();
            var result = factory.InstantiateClientScripts();
            Assert.True(result.Any());
        }

        [Fact]
        public void InstantiateLoggerWithIServiceLocator()
        {
            var loggerMock = new Mock<ILogger>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(lm => lm.GetInstance<ILogger>()).Returns(loggerMock.Object);

            var factory = new Factory(locatorMock.Object);
            var result = factory.InstantiateLogger();

            locatorMock.Verify(lm=>lm.GetInstance<ILogger>(), Times.Once());
            Assert.Equal(loggerMock.Object, result);
        }

        [Fact]
        public void InstantiateNullLogger()
        {
            var factory = new Factory();
            factory.Configuration = new GlimpseSection {Logging = {Level = LoggingLevel.Off}};

            var result = factory.InstantiateLogger();
            Assert.NotNull(result as NullLogger);
        }

        [Fact]
        public void InstantiateNLogLogger()
        {
            var factory = new Factory();
            factory.Configuration = new GlimpseSection { Logging = { Level = LoggingLevel.Warn } };

            var result = factory.InstantiateLogger();
            Assert.NotNull(result as NLogLogger);
        }

        [Fact]
        public void ReuseExistingLogger()
        {
            var logger = new NullLogger();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<ILogger>()).Returns(logger);

            var factory = new Factory(locatorMock.Object);

            var first = factory.InstantiateLogger();
            var second = factory.InstantiateLogger();

            Assert.Equal(logger, first);
            Assert.Equal(logger, second);
            locatorMock.Verify(l=>l.GetInstance<ILogger>(), Times.AtMostOnce());
        }

        [Fact]
        public void CascadeFromUserLocatorToProviderLocatorForSingleInstance()
        {
            var sequence = 0;
            var loggerMock = new Mock<ILogger>();
            var userLocatorMock = new Mock<IServiceLocator>();
            userLocatorMock.Setup(ul => ul.GetInstance<ILogger>()).Returns<ILogger>(null).Callback(()=>Assert.Equal(0, sequence++));
            var providerLocatorMock = new Mock<IServiceLocator>();
            providerLocatorMock.Setup(pl => pl.GetInstance<ILogger>()).Returns(loggerMock.Object).Callback(()=>Assert.Equal(1, sequence++));

            var factory = new Factory(providerLocatorMock.Object, userLocatorMock.Object);

            factory.InstantiateLogger();

            userLocatorMock.Verify(ul=>ul.GetInstance<ILogger>(), Times.Once());
            providerLocatorMock.Verify(pl=>pl.GetInstance<ILogger>(), Times.Once());
        }

        [Fact]
        public void CascadeFromUserLocatorToProviderLocatorForAllInstance()
        {
            var sequence = 0;
            var scripts = new List<IClientScript>();
            var userLocatorMock = new Mock<IServiceLocator>();
            userLocatorMock.Setup(ul => ul.GetAllInstances<IClientScript>()).Returns<ICollection<IClientScript>>(null).Callback(() => Assert.Equal(0, sequence++));
            var providerLocatorMock = new Mock<IServiceLocator>();
            providerLocatorMock.Setup(pl => pl.GetAllInstances<IClientScript>()).Returns(scripts).Callback(() => Assert.Equal(1, sequence++));

            var factory = new Factory(providerLocatorMock.Object, userLocatorMock.Object);

            factory.InstantiateClientScripts();

            userLocatorMock.Verify(ul => ul.GetAllInstances<IClientScript>(), Times.Once());
            providerLocatorMock.Verify(pl => pl.GetAllInstances<IClientScript>(), Times.Once());
        }

        [Fact]
        public void UseUserLocatorFirstForInstances()
        {
            var loggerMock = new Mock<ILogger>();
            var userLocatorMock = new Mock<IServiceLocator>();
            userLocatorMock.Setup(ul => ul.GetInstance<ILogger>()).Returns(loggerMock.Object);
            var providerLocatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(providerLocatorMock.Object, userLocatorMock.Object);

            factory.InstantiateLogger();

            userLocatorMock.Verify(ul => ul.GetInstance<ILogger>(), Times.Once());
            providerLocatorMock.Verify(pl => pl.GetInstance<ILogger>(), Times.Never());
        }

        [Fact]
        public void UseUserLocatorFirstForAllInstances()
        {
            var scripts = new List<IClientScript>();
            var userLocatorMock = new Mock<IServiceLocator>();
            userLocatorMock.Setup(ul => ul.GetAllInstances<IClientScript>()).Returns(scripts);
            var providerLocatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(providerLocatorMock.Object, userLocatorMock.Object);

            factory.InstantiateClientScripts();

            userLocatorMock.Verify(ul => ul.GetAllInstances<IClientScript>(), Times.Once());
            providerLocatorMock.Verify(pl => pl.GetAllInstances<IClientScript>(), Times.Never());
        }

        [Fact]
        public void LeverageConfigurationWhenCreatingDiscoverableCollection()
        {
            var path = @"c:\Windows";
            var config = new GlimpseSection {ClientScripts = {DiscoveryLocation = path, AutoDiscover = false}};

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<IClientScript>()).Returns<ICollection<IClientScript>>(null);

            var factory = new Factory(locatorMock.Object){Configuration = config};

            var result = factory.InstantiateClientScripts();

            var discoverableCollection = result as IDiscoverableCollection<IClientScript>;

            Assert.Equal(path, discoverableCollection.DiscoveryLocation);
        }
    }
}