using System;
using Glimpse.Core2;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class GlimpseConfigurationShould : IDisposable
    {
        private GlimpseConfigurationTester tester { get; set; }

        private GlimpseConfigurationTester Configuration
        {
            get { return tester ?? (tester = GlimpseConfigurationTester.Create()); }
            set { tester = value; }
        }

        [Fact]
        public void ConstructWithEndpointConfiguration()
        {
            var endpointConfigObj = Configuration.EndpointConfigMock.Object;

            var configuration = new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, endpointConfigObj);

            Assert.Equal(endpointConfigObj, configuration.ResourceEndpoint);
        }

        [Fact]
        public void ConstructWithFrameworkProvider()
        {
            var frameworkProviderObj = Configuration.FrameworkProviderMock.Object;

            var configuration = new GlimpseConfiguration(frameworkProviderObj, Configuration.EndpointConfigMock.Object);

            Assert.Equal(frameworkProviderObj, configuration.FrameworkProvider);
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
        public void CreateDefaultPersistanceStore()
        {
            var frameworkProviderObj = Configuration.FrameworkProviderMock.Object;

            var configuration = new GlimpseConfiguration(frameworkProviderObj, Configuration.EndpointConfigMock.Object);

            Assert.NotNull(configuration.PersistanceStore);
            Configuration.FrameworkProviderMock.Verify(fp => fp.HttpServerStore, Times.AtLeastOnce());
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
            Assert.NotNull(Configuration.ClientScripts);
        }

        [Fact]
        public void CreateDefaultSerializationConverters()
        {
            Assert.NotNull(Configuration.SerializationConverters);
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
        public void NotDiscoverSerlizationConverters()
        {
            Assert.Equal(0, Configuration.SerializationConverters.Count);
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullEndpointConfiguration()
        {
            Assert.Throws<ArgumentNullException>(
                () => new GlimpseConfiguration(Configuration.FrameworkProviderMock.Object, null));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullFrameworkProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(null, Configuration.EndpointConfigMock.Object));
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
        public void SerializationConvertersCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.SerializationConverters = null);
        }

        [Fact]
        public void DefaultToGlimpseModeOff()
        {
            Assert.Equal(RuntimePolicy.Off, Configuration.BasePolicy);
        }

        [Fact]
        public void ChangeGlimpseMode()
        {
            Configuration.BasePolicy = RuntimePolicy.ModifyResponseBody;

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Configuration.BasePolicy);
        }

        public void Dispose()
        {
            Configuration = null;
        }
    }
}