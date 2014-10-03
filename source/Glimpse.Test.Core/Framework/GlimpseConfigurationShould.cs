using System;
using System.Collections.Generic;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseConfigurationShould : IDisposable
    {
        private Glimpse.Core.Framework.Configuration sut;

        public GlimpseConfigurationShould()
        {
            var endpointConfig = new Mock<ResourceEndpointConfiguration>().Object;
            var persistenceStore = new Mock<IPersistenceStore>().Object;
            sut = new Glimpse.Core.Framework.Configuration(endpointConfig, persistenceStore);
        }

        public void Dispose()
        {
            sut = null;
        }

        [Fact]
        public void CreateDefaultHtmlEncoder()
        {
            Assert.NotNull(sut.HtmlEncoder);
        }

        [Fact]
        public void CreateDefaultLogger()
        {
            Assert.NotNull(sut.Logger);
        }

        [Fact]
        public void CreateDefaultProxyFactory()
        {
            Assert.NotNull(sut.ProxyFactory);
        }

        [Fact]
        public void CreateDefaultInspectorsCollection()
        {
            Assert.NotNull(sut.Inspectors);
        }

        [Fact]
        public void CreateDefaultResourcesCollection()
        {
            Assert.NotNull(sut.Resources);
        }

        [Fact]
        public void CreateDefaultTabsCollection()
        {
            Assert.NotNull(sut.Tabs);
        }

        [Fact]
        public void CreateDefaultValidatorsCollection()
        {
            Assert.NotNull(sut.RuntimePolicies);
        }

        [Fact]
        public void HtmlEncoderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.HtmlEncoder = null);
        }

        [Fact]
        public void LogerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Logger = null);
        }

        [Fact]
        public void PersistenceStoreCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.PersistenceStore = null);
        }

        [Fact]
        public void InspectorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Inspectors = null);
        }

        [Fact]
        public void ResourceEndpointCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.ResourceEndpoint = null);
        }

        [Fact]
        public void ResourcesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Resources = null);
        }

        [Fact]
        public void ProxyFactoryCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.ProxyFactory = null);
        }

        [Fact]
        public void SerializerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Serializer = null);
        }

        [Fact]
        public void TabsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Tabs = null);
        }

        [Fact]
        public void MessageBrokerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.MessageBroker = null);
        }

        [Fact]
        public void ValidatorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.RuntimePolicies = null);
        }

        [Fact]
        public void ClientScriptsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.ClientScripts = null);
        }

        [Fact]
        public void DefaultResourceNameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.DefaultResource = null);
        }

        [Fact]
        public void ChangeGlimpseMode()
        {
            sut.DefaultRuntimePolicy = RuntimePolicy.ModifyResponseBody;

            Assert.Equal(RuntimePolicy.ModifyResponseBody, sut.DefaultRuntimePolicy);
        }

        [Theory, AutoMock]
        public void ThrowExceptionWhenConstructedWithNullResourceEndpointConfiguration(IPersistenceStore persistenceStore)
        {
            Assert.Throws<ArgumentNullException>(()=>new Glimpse.Core.Framework.Configuration(null, persistenceStore));
        }

        [Theory, AutoMock]
        public void ThrowExceptionWhenConstructedWithNullPersistenceStore(ResourceEndpointConfiguration endpoingConfiguration)
        {
            Assert.Throws<ArgumentNullException>(() => new Glimpse.Core.Framework.Configuration(endpoingConfiguration, null));
        }

        [Fact]
        public void GetDefaultHtmlEncoderWithoutServiceLocator()
        {
            Assert.NotNull(sut.HtmlEncoder);
        }

        [Fact]
        public void GetDefaultMessageBrokerWithoutServiceLocator()
        {
            Assert.NotNull(sut.MessageBroker);
        }

        [Fact]
        public void GetDefaultProxyFactoryWithoutServiceLocator()
        {
            Assert.NotNull(sut.ProxyFactory);
        }

        [Fact]
        public void GetDefaultSerializerWithoutServiceLocator()
        {
            Assert.NotNull(sut.Serializer);
        }

        [Fact]
        public void GetDefaultDefaultResourceWithoutServiceLocator()
        {
            Assert.NotNull(sut.DefaultResource);
        }

        [Fact]
        public void GetBasePolicyFromConfiguration()
        {
            Assert.Equal(RuntimePolicy.On, sut.DefaultRuntimePolicy);
        }

        [Fact]
        public void GetDefaultClientScriptsWithoutServiceLocator()
        {
            Assert.NotNull(sut.ClientScripts);
        }

        [Fact]
        public void GetDefaultSerializationConvertersWithoutServiceLocator()
        {
            Assert.NotNull(sut.SerializationConverters);
        }

        [Fact]
        public void GetNullLoggerWithoutServiceLocator()
        {
            sut.XmlConfiguration = new Section();

            Assert.NotNull(sut.Logger as NullLogger);
        }

        [Fact]
        public void GetDefaultLoggerWithoutServiceLocator()
        {
            sut.XmlConfiguration = new Section { Logging = { Level = LoggingLevel.Warn } };

            Assert.NotNull(sut.Logger);
        }
    }
}