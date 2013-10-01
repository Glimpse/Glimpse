using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core;
using Glimpse.Test.Common;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseConfigurationShould : IDisposable
    {
        private GlimpseConfigurationTester tester { get; set; }

        private GlimpseConfigurationTester Configuration
        {
            get { return tester ?? (tester = GlimpseConfigurationTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Configuration = null;
        }

        [Fact]
        public void CreateDefaultHtmlEncoder()
        {
            Assert.NotNull(Configuration.HtmlEncoder);
        }

        [Fact]
        public void CreateDefaultLogger()
        {
            Assert.NotNull(Configuration.Logger);
        }

        [Fact]
        public void Construct()
        {
            var providerMock = new Mock<IFrameworkProvider>();
            var loggerMock = new Mock<ILogger>();
            var resourcesStub = new LinkedList<IResource>();
            var tabsStub = new List<ITab>();
            var policiesStub = new List<IRuntimePolicy>();

            var config = new GlimpseConfiguration(providerMock.Object, loggerMock.Object, resourcesStub, 
                tabsStub, policiesStub);

            Assert.Equal(providerMock.Object, config.FrameworkProvider);
            Assert.Equal(loggerMock.Object, config.Logger);
            Assert.Equal(resourcesStub, config.Resources);
            Assert.Equal(tabsStub, config.Tabs);
            Assert.Equal(policiesStub, config.RuntimePolicies);
            
        }

        [Fact]
        public void CreateDefaultProxyFactory()
        {
            Assert.NotNull(Configuration.ProxyFactory);
        }

        [Fact]
        public void CreateDefaultInspectorsCollection()
        {
            Assert.NotNull(Configuration.Inspectors);
        }

        [Fact]
        public void CreateDefaultResourcesCollection()
        {
            Assert.NotNull(Configuration.Resources);
        }

        [Fact]
        public void CreateDefaultTabsCollection()
        {
            Assert.NotNull(Configuration.Tabs);
        }

        [Fact]
        public void CreateDefaultValidatorsCollection()
        {
            Assert.NotNull(Configuration.RuntimePolicies);
        }

        [Fact]
        public void NotDiscoverResources()
        {
            Assert.Equal(0, Configuration.Resources.Count);
        }

        [Fact]
        public void NotDiscoverTabs()
        {
            Assert.Equal(0, Configuration.Tabs.Count);
        }

        [Fact]
        public void NotDiscoverValidators()
        {
            Assert.Equal(0, Configuration.RuntimePolicies.Count);
        }


        [Fact]
        public void FrameworkProviderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.FrameworkProvider = null);
        }

        [Fact]
        public void HtmlEncoderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.HtmlEncoder = null);
        }

        [Fact]
        public void LogerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Logger = null);
        }

        [Fact]
        public void PersistenceStoreCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.PersistenceStore = null);
        }

        [Fact]
        public void InspectorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Inspectors = null);
        }

        [Fact]
        public void ResourceEndpointCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ResourceEndpoint = null);
        }

        [Fact]
        public void ResourcesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Resources = null);
        }

        [Fact]
        public void ProxyFactoryCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ProxyFactory = null);
        }

        [Fact]
        public void SerializerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Serializer = null);
        }

        [Fact]
        public void TabsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Tabs = null);
        }

        [Fact]
        public void MessageBrokerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.MessageBroker = null);
        }

        [Fact]
        public void ValidatorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.RuntimePolicies = null);
        }

        [Fact]
        public void ClientScriptsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ClientScripts = null);
        }

        [Fact]
        public void DefaultResourceNameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.DefaultResource = null);
        }

        [Fact]
        public void ChangeGlimpseMode()
        {
            Configuration.DefaultRuntimePolicy = RuntimePolicy.ModifyResponseBody;

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Configuration.DefaultRuntimePolicy);
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullFrameworkProvider()
        {
            Assert.Throws<ArgumentNullException>(()=>new GlimpseConfiguration(null, Configuration.LoggerMock.Object, new List<IResource>(), new List<ITab>(), new List<IRuntimePolicy>()));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, null, new List<IResource>(), new List<ITab>(), new List<IRuntimePolicy>()));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullResource()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.LoggerMock.Object, null, new List<ITab>(), new List<IRuntimePolicy>()));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullTabs()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.LoggerMock.Object, new List<IResource>(), null, new List<IRuntimePolicy>()));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullRuntimePolicies()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.LoggerMock.Object, new List<IResource>(), new List<ITab>(), null));
        }

        [Theory, AutoMock]
        public void GetDefaultHtmlEncoderWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.HtmlEncoder;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetHtmlEncoderFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, IHtmlEncoder expected)
        {
            serviceLocator.Setup(sl => sl.GetInstance<IHtmlEncoder>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.HtmlEncoder;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetInstance<IHtmlEncoder>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetDefaultMessageBrokerWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.MessageBroker;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetMessageBrokerFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, IMessageBroker expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IMessageBroker>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.MessageBroker;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetInstance<IMessageBroker>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetDefaultProxyFactoryWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.ProxyFactory;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetProxyFactoryFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, IProxyFactory expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IProxyFactory>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.ProxyFactory;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoMock]
        public void GetDefaultSerializerWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.Serializer;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetSerializerFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ISerializer expected)
        {
            serviceLocator.Setup(l => l.GetInstance<ISerializer>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.Serializer;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetInstance<ISerializer>(), Times.Once());
        }

        [Theory, AutoMock]
        public void ThrowExceptionWhenResourceEndpointConfigSetWithNull(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            Assert.Throws<ArgumentNullException>(() => sut.ResourceEndpoint = null);
        }

        [Theory, AutoMock]
        public void GetDefaultDefaultResourceWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.DefaultResource;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetDefaultResourceFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, IResource expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IResource>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.DefaultResource;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetInstance<IResource>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetBasePolicyFromConfiguration(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.DefaultRuntimePolicy;

            Assert.Equal(RuntimePolicy.On, actual);
        }

        [Theory, AutoMock]
        public void ThrowExceptionWhenPersistanceStoreSetWithNull(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            Assert.Throws<ArgumentNullException>(() => sut.PersistenceStore = null);
        }

        [Theory, AutoMock]
        public void ThrowExceptionWhenConstructedWithNullResourceEndpointConfig(ResourceEndpointConfiguration endpointConfiguration)
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(endpointConfiguration, null));
        }

        [Theory, AutoMock]
        public void GetDefaultClientScriptsWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.ClientScripts;
            Assert.NotEmpty(actual);
        }

        [Theory, AutoMock]
        public void GetClientScriptsFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<IClientScript> expected)
        {
            serviceLocator.Setup(ul => ul.GetAllInstances<IClientScript>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.ClientScripts;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(ul => ul.GetAllInstances<IClientScript>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetDefaultInspectorsWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.Inspectors;

            Assert.NotEmpty(actual);
        }

        [Theory, AutoMock]
        public void LeverageServiceLocatorForInspectors(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<IInspector> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IInspector>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.Inspectors;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoMock]
        public void GetDefaultSerializationConvertersWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.SerializationConverters;

            Assert.NotEmpty(actual);
        }

        [Theory, AutoMock]
        public void GetServiceLocatorFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<ISerializationConverter> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<ISerializationConverter>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.SerializationConverters;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetAllInstances<ISerializationConverter>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetNullLoggerWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                XmlConfiguration = new Section()
            };

            var actual = sut.Logger;

            Assert.NotNull(actual as NullLogger);
        }

        [Theory, AutoMock]
        public void GetDefaultLoggerWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                XmlConfiguration = new Section { Logging = { Level = LoggingLevel.Warn } }
            };

            var actual = sut.Logger;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetLoggerFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ILogger expected)
        {
            serviceLocator.Setup(ul => ul.GetInstance<ILogger>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.Logger;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(ul => ul.GetInstance<ILogger>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetDefaultTabsWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.Tabs;

            Assert.NotNull(actual);
        }

        [Theory, AutoMock]
        public void GetTabsFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<ITab> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<ITab>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.Tabs;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetAllInstances<ITab>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetDefaultResourcesWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual = sut.Resources;

            Assert.NotEmpty(actual);
        }

        [Theory, AutoMock]
        public void GetResourcesFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<IResource> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IResource>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };
            
            var actual = sut.Resources;

            Assert.Equal(expected, actual);
        }

        [Theory, AutoMock]
        public void GetDefaultRuntimePoliciesWithoutServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore)
        {
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore);

            var actual  = sut.RuntimePolicies;

            Assert.NotEmpty(actual);
        }

        [Theory, AutoMock]
        public void GetDefaultRuntimePoliciesFromServiceLocator(ResourceEndpointConfiguration endpointConfiguration, IPersistenceStore persistenceStore, IServiceLocator serviceLocator, ICollection<IRuntimePolicy> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IRuntimePolicy>()).Returns(expected);
            var sut = new GlimpseConfiguration(endpointConfiguration, persistenceStore)
            {
                UserServiceLocator = serviceLocator
            };

            var actual = sut.RuntimePolicies;

            Assert.Equal(expected, actual);
            serviceLocator.Verify(l => l.GetAllInstances<IRuntimePolicy>(), Times.Once());
        }
    }
}