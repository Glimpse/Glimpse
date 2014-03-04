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

        [Theory, AutoMock]
        public void GetHtmlEncoderFromServiceLocator(IServiceLocator serviceLocator, IHtmlEncoder expected)
        {
            serviceLocator.Setup(sl => sl.GetInstance<IHtmlEncoder>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.HtmlEncoder);
            serviceLocator.Verify(l => l.GetInstance<IHtmlEncoder>(), Times.Once());
        }

        [Fact]
        public void GetDefaultMessageBrokerWithoutServiceLocator()
        {
            Assert.NotNull(sut.MessageBroker);
        }

        [Theory, AutoMock]
        public void GetMessageBrokerFromServiceLocator(IServiceLocator serviceLocator, IMessageBroker expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IMessageBroker>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.MessageBroker);
            serviceLocator.Verify(l => l.GetInstance<IMessageBroker>(), Times.Once());
        }

        [Fact]
        public void GetDefaultProxyFactoryWithoutServiceLocator()
        {
            Assert.NotNull(sut.ProxyFactory);
        }

        [Theory, AutoMock]
        public void GetProxyFactoryFromServiceLocator(IServiceLocator serviceLocator, IProxyFactory expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IProxyFactory>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.ProxyFactory);
        }

        [Fact]
        public void GetDefaultSerializerWithoutServiceLocator()
        {
            Assert.NotNull(sut.Serializer);
        }

        [Theory, AutoMock]
        public void GetSerializerFromServiceLocator(IServiceLocator serviceLocator, ISerializer expected)
        {
            serviceLocator.Setup(l => l.GetInstance<ISerializer>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.Serializer);
            serviceLocator.Verify(l => l.GetInstance<ISerializer>(), Times.Once());
        }

        [Fact]
        public void GetDefaultDefaultResourceWithoutServiceLocator()
        {
            Assert.NotNull(sut.DefaultResource);
        }

        [Theory, AutoMock]
        public void GetDefaultResourceFromServiceLocator(IServiceLocator serviceLocator, IResource expected)
        {
            serviceLocator.Setup(l => l.GetInstance<IResource>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.DefaultResource);
            serviceLocator.Verify(l => l.GetInstance<IResource>(), Times.Once());
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

        [Theory, AutoMock]
        public void GetClientScriptsFromServiceLocator(IServiceLocator serviceLocator, ICollection<IClientScript> expected)
        {
            serviceLocator.Setup(ul => ul.GetAllInstances<IClientScript>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.ClientScripts);
            serviceLocator.Verify(ul => ul.GetAllInstances<IClientScript>(), Times.Once());
        }

        [Theory, AutoMock]
        public void LeverageServiceLocatorForInspectors(IServiceLocator serviceLocator, ICollection<IInspector> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IInspector>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.Inspectors);
        }

        [Fact]
        public void GetDefaultSerializationConvertersWithoutServiceLocator()
        {
            Assert.NotNull(sut.SerializationConverters);
        }

        [Theory, AutoMock]
        public void GetServiceLocatorFromServiceLocator(IServiceLocator serviceLocator, ICollection<ISerializationConverter> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<ISerializationConverter>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.SerializationConverters);
            serviceLocator.Verify(l => l.GetAllInstances<ISerializationConverter>(), Times.Once());
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

        [Theory, AutoMock]
        public void GetLoggerFromServiceLocator(IServiceLocator serviceLocator, ILogger expected)
        {
            serviceLocator.Setup(ul => ul.GetInstance<ILogger>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.Logger);
            serviceLocator.Verify(ul => ul.GetInstance<ILogger>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetTabsFromServiceLocator(IServiceLocator serviceLocator, ICollection<ITab> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<ITab>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.Tabs);
            serviceLocator.Verify(l => l.GetAllInstances<ITab>(), Times.Once());
        }

        [Theory, AutoMock]
        public void GetResourcesFromServiceLocator(IServiceLocator serviceLocator, ICollection<IResource> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IResource>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.Resources);
        }

        [Theory, AutoMock]
        public void GetDefaultRuntimePoliciesFromServiceLocator(IServiceLocator serviceLocator, ICollection<IRuntimePolicy> expected)
        {
            serviceLocator.Setup(l => l.GetAllInstances<IRuntimePolicy>()).Returns(expected);
            sut.UserServiceLocator = serviceLocator;

            Assert.Equal(expected, sut.RuntimePolicies);
            serviceLocator.Verify(l => l.GetAllInstances<IRuntimePolicy>(), Times.Once());
        }
    }
}