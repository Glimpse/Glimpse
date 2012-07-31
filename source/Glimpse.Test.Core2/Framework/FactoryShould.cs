using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Configuration;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
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
        public void InstantiateFrameworkProviderCachesResults()
        {
            var providerMock = new Mock<IFrameworkProvider>();
            var serviceLocatorMock = new Mock<IServiceLocator>();

            serviceLocatorMock.Setup(sl => sl.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);

            var factory = new Factory(serviceLocatorMock.Object);
            var first = factory.InstantiateFrameworkProvider();
            var second = factory.InstantiateFrameworkProvider();
            Assert.Equal(providerMock.Object, first);
            Assert.Equal(providerMock.Object, second);
            serviceLocatorMock.Verify(sl=>sl.GetInstance<IFrameworkProvider>(), Times.AtMostOnce());
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
            var result = factory.InstantiateResourceEndpointConfiguration();
            Assert.Equal(endpointConfigMock.Object, result);
        }

        [Fact]
        public void InstantiateResourceEndpointConfigWithoutIServiceLocator()
        {
            var factory = new Factory();
            Assert.Throws<GlimpseException>(() => factory.InstantiateResourceEndpointConfiguration());
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

        [Fact]
        public void GetBasePolicyFromConfiguration()
        {
            var locatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(locatorMock.Object);

            RuntimePolicy result = factory.InstantiateDefaultRuntimePolicy();

            Assert.Equal(RuntimePolicy.On, result);
        }

        [Fact]
        public void InstantiateHtmlEncoderWithAntiXss()
        {
            var locatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(locatorMock.Object);

            IHtmlEncoder encoder = factory.InstantiateHtmlEncoder();

            Assert.NotNull(encoder);
            Assert.NotNull(encoder as AntiXssEncoder);
            locatorMock.Verify(l=>l.GetInstance<IHtmlEncoder>(), Times.Once());
        }

        [Fact]
        public void LeverageServiceLocatorForHtmlEncoder()
        {
            var encoderMock = new Mock<IHtmlEncoder>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IHtmlEncoder>()).Returns(encoderMock.Object);

            var factory = new Factory(locatorMock.Object);

            IHtmlEncoder encoder = factory.InstantiateHtmlEncoder();

            Assert.NotNull(encoder);
            Assert.Equal(encoderMock.Object, encoder);
            
            locatorMock.Verify(l => l.GetInstance<IHtmlEncoder>(), Times.Once());
        }

        [Fact]
        public void InstantiatePersistanceStoreWithApplicationPersistanceStore()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var providerMock = new Mock<IFrameworkProvider>();
            providerMock.Setup(pm => pm.HttpServerStore).Returns(dataStoreMock.Object);
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);

            var factory = new Factory(locatorMock.Object);

            IPersistanceStore store = factory.InstantiatePersistanceStore();

            Assert.NotNull(store);
            Assert.NotNull(store as ApplicationPersistanceStore);
        }

        [Fact]
        public void LeverageServiceLocatorForPersistanceStore()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var providerMock = new Mock<IFrameworkProvider>();
            providerMock.Setup(pm => pm.HttpServerStore).Returns(dataStoreMock.Object);
            var persistanceStoreMock = new Mock<IPersistanceStore>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);
            locatorMock.Setup(l => l.GetInstance<IPersistanceStore>()).Returns(persistanceStoreMock.Object);

            var factory = new Factory(locatorMock.Object);

            IPersistanceStore store = factory.InstantiatePersistanceStore();

            Assert.NotNull(store);
            Assert.Equal(persistanceStoreMock.Object, store);
        }

        [Fact]
        public void InstantiatePipelineInspectorsWithReflectionDiscoverableCollection()
        {
            var locatorMock = new Mock<IServiceLocator>();
            var factory = new Factory(locatorMock.Object);
            ICollection<IPipelineInspector> inspectors = factory.InstantiatePipelineInspectors();

            Assert.NotNull(inspectors);
            Assert.NotNull(inspectors as ReflectionDiscoverableCollection<IPipelineInspector>);
        }

        [Fact]
        public void LeverageServiceLocatorForPipelineInspectors()
        {
            ICollection<IPipelineInspector> inspectors = new List<IPipelineInspector>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<IPipelineInspector>()).Returns(inspectors);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiatePipelineInspectors();

            Assert.Equal(inspectors, result);
        }

        [Fact]
        public void InstantiateResourcesWithReflectionDiscoverableCollection()
        {
            var locatorMock = new Mock<IServiceLocator>();
            var factory = new Factory(locatorMock.Object);
            ICollection<IResource> resources = factory.InstantiateResources();

            Assert.NotNull(resources);
            Assert.NotNull(resources as ReflectionDiscoverableCollection<IResource>);
        }

        [Fact]
        public void LeverageServiceLocatorForResources()
        {
            ICollection<IResource> resources = new List<IResource>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<IResource>()).Returns(resources);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateResources();

            Assert.Equal(resources, result);
        }

        [Fact]
        public void InstantiateSerializerWithJsonNetSerializer()
        {
            var locatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(locatorMock.Object);

            ISerializer serializer = factory.InstantiateSerializer();

            Assert.NotNull(serializer);
            Assert.NotNull(serializer as JsonNetSerializer);
            locatorMock.Verify(l => l.GetInstance<ISerializer>(), Times.Once());
        }

        [Fact]
        public void LeverageServiceLocatorForSerializer()
        {
            var serializerMock = new Mock<ISerializer>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<ISerializer>()).Returns(serializerMock.Object);

            var factory = new Factory(locatorMock.Object);

            ISerializer serializer = factory.InstantiateSerializer();

            Assert.NotNull(serializer);
            Assert.Equal(serializerMock.Object, serializer);

            locatorMock.Verify(l => l.GetInstance<ISerializer>(), Times.Once());
        }

        [Fact]
        public void RegisterConvertersWithSerializer()
        {
            var converters = new List<ISerializationConverter>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<ISerializationConverter>()).Returns(converters);

            var factory = new Factory(locatorMock.Object);

            factory.InstantiateSerializer();

            locatorMock.Verify(l=>l.GetAllInstances<ISerializationConverter>(), Times.Once());
        }

        [Fact]
        public void InstantiateTabsWithReflectionDiscoverableCollection()
        {
            var locatorMock = new Mock<IServiceLocator>();
            var factory = new Factory(locatorMock.Object);
            ICollection<ITab> tabs = factory.InstantiateTabs();

            Assert.NotNull(tabs);
            Assert.NotNull(tabs as ReflectionDiscoverableCollection<ITab>);
        }

        [Fact]
        public void LeverageServiceLocatorForTabs()
        {
            ICollection<ITab> tabs = new List<ITab>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<ITab>()).Returns(tabs);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateTabs();

            Assert.Equal(tabs, result);
            locatorMock.Verify(l => l.GetAllInstances<ITab>(), Times.Once());
        }

        [Fact]
        public void InstantiateRuntimePoliciesWithReflectionDiscoverableCollection()
        {
            var locatorMock = new Mock<IServiceLocator>();
            var factory = new Factory(locatorMock.Object);
            ICollection<IRuntimePolicy> policies = factory.InstantiateRuntimePolicies();

            Assert.NotNull(policies);
            Assert.NotNull(policies as ReflectionDiscoverableCollection<IRuntimePolicy>);
        }

        [Fact]
        public void LeverageServiceLocatorForRuntimePolicies()
        {
            ICollection<IRuntimePolicy> policies = new List<IRuntimePolicy>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<IRuntimePolicy>()).Returns(policies);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateRuntimePolicies();

            Assert.Equal(policies, result);
            locatorMock.Verify(l => l.GetAllInstances<IRuntimePolicy>(), Times.Once());
        }

        [Fact]
        public void InstantiateSerializationConvertersWithReflectionDiscoverableCollection()
        {
            var locatorMock = new Mock<IServiceLocator>();
            var factory = new Factory(locatorMock.Object);
            ICollection<ISerializationConverter> converters = factory.InstantiateSerializationConverters();

            Assert.NotNull(converters);
            Assert.NotNull(converters as ReflectionDiscoverableCollection<ISerializationConverter>);
        }

        [Fact]
        public void LeverageServiceLocatorForSerializationConverters()
        {
            ICollection<ISerializationConverter> converters = new List<ISerializationConverter>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetAllInstances<ISerializationConverter>()).Returns(converters);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateSerializationConverters();

            Assert.Equal(converters, result);
            locatorMock.Verify(l => l.GetAllInstances<ISerializationConverter>(), Times.Once());
        }

        [Fact]
        public void InstantiateDefaultResourceWithConfigurationResource()
        {
            var locatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(locatorMock.Object);

            IResource resource = factory.InstantiateDefaultResource();

            Assert.NotNull(resource);
            Assert.NotNull(resource as Glimpse.Core2.Resource.ConfigurationResource);
            locatorMock.Verify(l => l.GetInstance<IResource>(), Times.Once());
        }

        [Fact]
        public void LeverageServiceLocatorForDefaultResource()
        {
            var resourceMock = new Mock<IResource>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IResource>()).Returns(resourceMock.Object);

            var factory = new Factory(locatorMock.Object);

            IResource resource = factory.InstantiateDefaultResource();

            Assert.NotNull(resource);
            Assert.Equal(resourceMock.Object, resource);

            locatorMock.Verify(l => l.GetInstance<IResource>(), Times.Once());
        }

        [Fact]
        public void InstantiateGlimpseConfiguration()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var providerMock = new Mock<IFrameworkProvider>();
            providerMock.Setup(p => p.HttpServerStore).Returns(dataStoreMock.Object);
            var endpointConfigMock = new Mock<ResourceEndpointConfiguration>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);
            locatorMock.Setup(l => l.GetInstance<ResourceEndpointConfiguration>()).Returns(endpointConfigMock.Object);

            var factory = new Factory(locatorMock.Object);

            IGlimpseConfiguration resource = factory.InstantiateConfiguration();

            Assert.NotNull(resource);
            locatorMock.Verify(l => l.GetInstance<IResource>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<IFrameworkProvider>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<ResourceEndpointConfiguration>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<ILogger>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<IHtmlEncoder>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<IPersistanceStore>(), Times.Once());
            locatorMock.Verify(l => l.GetInstance<ISerializer>(), Times.Once());
            locatorMock.Verify(l => l.GetAllInstances<IClientScript>(), Times.Once());
            locatorMock.Verify(l => l.GetAllInstances<IPipelineInspector>(), Times.Once());
            locatorMock.Verify(l => l.GetAllInstances<IResource>(), Times.Once());
            locatorMock.Verify(l => l.GetAllInstances<ITab>(), Times.Once());
            locatorMock.Verify(l => l.GetAllInstances<IRuntimePolicy>(), Times.Once());
        }

        [Fact]
        public void LeverageServiceLocatorForGlimpseConfiguration()
        {
            var configMock = new Mock<IGlimpseConfiguration>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IGlimpseConfiguration>()).Returns(configMock.Object);

            var factory = new Factory(locatorMock.Object);

            IGlimpseConfiguration config = factory.InstantiateConfiguration();

            Assert.NotNull(config);
            Assert.Equal(configMock.Object, config);

            locatorMock.Verify(l => l.GetInstance<IGlimpseConfiguration>(), Times.Once());
        }

        [Fact]
        public void InstantiateRuntimeReturnsDefaultRuntime()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var providerMock = new Mock<IFrameworkProvider>();
            providerMock.Setup(p => p.HttpServerStore).Returns(dataStoreMock.Object);
            var endpointConfigMock = new Mock<ResourceEndpointConfiguration>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);
            locatorMock.Setup(l => l.GetInstance<ResourceEndpointConfiguration>()).Returns(endpointConfigMock.Object);

            var factory = new Factory(locatorMock.Object);

            var runtime = factory.InstantiateRuntime();

            Assert.NotNull(runtime);
            Assert.NotNull(runtime as GlimpseRuntime);
        }

        [Fact]
        public void LeverageServiceLocatorForRuntime()
        {
            var runtimeMock = new Mock<IGlimpseRuntime>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IGlimpseRuntime>()).Returns(runtimeMock.Object);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateRuntime();

            Assert.NotNull(result);
            Assert.Equal(runtimeMock.Object, result);
        }

        [Fact]
        public void SetUserServiceLocatorWhenConfigured()
        {
            var factory = new Factory();

            Assert.NotNull(factory.UserServiceLocator);
            Assert.True(factory.UserServiceLocator.GetType() == typeof(DummyServiceLocator));
        }

        [Fact]
        public void LeaveUserServiceLocatorAsBlankWhenNotConfigured()
        {
            var factory = new Factory(null, null, new GlimpseSection());

            Assert.Null(factory.UserServiceLocator);
        }

        [Fact]
        public void InstantiateProxyFactoryReturnsCastleDynamicProxyFactory()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var providerMock = new Mock<IFrameworkProvider>();
            providerMock.Setup(p => p.HttpServerStore).Returns(dataStoreMock.Object);
            var endpointConfigMock = new Mock<ResourceEndpointConfiguration>();

            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IFrameworkProvider>()).Returns(providerMock.Object);
            locatorMock.Setup(l => l.GetInstance<ResourceEndpointConfiguration>()).Returns(endpointConfigMock.Object);

            var factory = new Factory(locatorMock.Object);

            var proxyFactory = factory.InstantiateProxyFactory();

            Assert.NotNull(proxyFactory);
            Assert.NotNull(proxyFactory as CastleDynamicProxyFactory);
        }

        [Fact]
        public void LeverageServiceLocatorForProxyFactory()
        {
            var proxyFactoryMock = new Mock<IProxyFactory>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IProxyFactory>()).Returns(proxyFactoryMock.Object);

            var factory = new Factory(locatorMock.Object);

            var result = factory.InstantiateProxyFactory();

            Assert.NotNull(result);
            Assert.Equal(proxyFactoryMock.Object, result);
        }


        [Fact]
        public void InstantiateMessageBrokerWithDefault()
        {
            var locatorMock = new Mock<IServiceLocator>();

            var factory = new Factory(locatorMock.Object);

            IMessageBroker broker = factory.InstantiateMessageBroker();

            Assert.NotNull(broker);
            Assert.NotNull(broker as MessageBroker);
            locatorMock.Verify(l => l.GetInstance<IMessageBroker>(), Times.Once());
        }

        [Fact]
        public void LeverageServiceLocatorForMessageBroker()
        {
            var brokerMock = new Mock<IMessageBroker>();
            var locatorMock = new Mock<IServiceLocator>();
            locatorMock.Setup(l => l.GetInstance<IMessageBroker>()).Returns(brokerMock.Object);

            var factory = new Factory(locatorMock.Object);

            IMessageBroker broker = factory.InstantiateMessageBroker();

            Assert.NotNull(broker);
            Assert.Equal(brokerMock.Object, broker);

            locatorMock.Verify(l => l.GetInstance<IMessageBroker>(), Times.Once());
        }
    }
}