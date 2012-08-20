using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

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
            var endpointConfogMock = new Mock<ResourceEndpointConfiguration>();
            var clientScriptsStub = new List<IClientScript>();
            var loggerMock = new Mock<ILogger>();
            var encoderMock = new Mock<IHtmlEncoder>();
            var storeMock = new Mock<IPersistanceStore>();
            var inspectorsStub = new LinkedList<IPipelineInspector>();
            var resourcesStub = new LinkedList<IResource>();
            var serializerMock = new Mock<ISerializer>();
            var tabsStub = new List<ITab>();
            var policiesStub = new List<IRuntimePolicy>();
            var defaultResourceMock = new Mock<IResource>();
            var factoryMock = new Mock<IProxyFactory>();
            var brokerMock = new Mock<IMessageBroker>();

            var config = new GlimpseConfiguration(providerMock.Object, endpointConfogMock.Object, clientScriptsStub, loggerMock.Object, RuntimePolicy.On, encoderMock.Object, storeMock.Object, inspectorsStub, resourcesStub, serializerMock.Object, 
                tabsStub, policiesStub, defaultResourceMock.Object, factoryMock.Object, brokerMock.Object, "~/Glimpse.axd");

            Assert.Equal(providerMock.Object, config.FrameworkProvider);
            Assert.Equal(endpointConfogMock.Object, config.ResourceEndpoint);
            Assert.Equal(clientScriptsStub, config.ClientScripts);
            Assert.Equal(loggerMock.Object, config.Logger);
            Assert.Equal(encoderMock.Object, config.HtmlEncoder);
            Assert.Equal(storeMock.Object, config.PersistanceStore);
            Assert.Equal(inspectorsStub, config.PipelineInspectors);
            Assert.Equal(resourcesStub, config.Resources);
            Assert.Equal(serializerMock.Object, config.Serializer);
            Assert.Equal(tabsStub, config.Tabs);
            Assert.Equal(policiesStub, config.RuntimePolicies);
            Assert.Equal(defaultResourceMock.Object, config.DefaultResource);
            Assert.Equal(factoryMock.Object, config.ProxyFactory);
            Assert.Equal(brokerMock.Object, config.MessageBroker);
        }

        [Fact]
        public void CreateDefaultProxyFactory()
        {
            Assert.NotNull(Configuration.ProxyFactory);
        }

        [Fact]
        public void CreateDefaultPiplineInspectorsCollection()
        {
            Assert.NotNull(Configuration.PipelineInspectors);
        }

        [Fact]
        public void CreateDefaultResourcesCollection()
        {
            Assert.NotNull(Configuration.Resources);
        }

        [Fact]
        public void CreateDefaultSerializer()
        {
            Assert.NotNull(Configuration.Serializer);
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
        public void CreateDefaultClientScripts()
        {
            Assert.NotNull(Configuration.ClientScriptsStub);
        }

        [Fact]
        public void NotDiscoverPipelineInspectors()
        {
            Assert.Equal(0, Configuration.PipelineInspectors.Count);
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
        public void PersistanceStoreCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.PersistanceStore = null);
        }

        [Fact]
        public void PipelineInspectorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.PipelineInspectors = null);
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
            Assert.Throws<ArgumentNullException>(()=>new GlimpseConfiguration(null, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullEndpointConfig()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, null, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullClientScripts()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, null, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, null, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullHtmlEncoder()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, null, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullRersistanceStore()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, null, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullPipelineInspectors()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, null, new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullResource()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), null, Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullSerializer()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), null, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullTabs()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, null, new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullRuntimePolicies()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), null, Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullDefaultResource()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), null, Configuration.ProxyFactoryMock.Object, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullProxyFactory()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, null, Configuration.MessageBrokerMock.Object, "~/Glimpse.axd"));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullMessageBroker()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, Configuration.EndpointConfigMock.Object, Configuration.ClientScriptsStub, Configuration.LoggerMock.Object, RuntimePolicy.Off, Configuration.HtmlEncoderMock.Object, Configuration.PersistanceStoreMock.Object, new List<IPipelineInspector>(), new List<IResource>(), Configuration
                .SerializerMock.Object, new List<ITab>(), new List<IRuntimePolicy>(), Configuration.DefaultResourceMock.Object, Configuration.ProxyFactoryMock.Object, null, "~/Glimpse.axd"));
        }
       
    }
}