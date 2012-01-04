using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Extensions;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class GlimpseConfigurationShould:IDisposable
    {

        public GlimpseConfigurationShould()
        {
            FrameworkProviderMock = new Mock<IFrameworkProvider>().Setup();
            EndpointConfigMock = new Mock<IGlimpseResourceEndpointConfiguration>();
        }


        private GlimpseConfiguration configuration;
        private GlimpseConfiguration Configuration
        {
            get { return configuration ?? (configuration = new GlimpseConfiguration(FrameworkProviderMock.Object, EndpointConfigMock.Object)); }
            set { configuration = value; }
        }

        private Mock<IGlimpseResourceEndpointConfiguration> EndpointConfigMock { get; set; }

        private Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }




        [Fact]
        public void ConstructWithEndpointConfiguration()
        {
            var endpointConfigObj = EndpointConfigMock.Object;

            var configuration = new GlimpseConfiguration(FrameworkProviderMock.Object, endpointConfigObj);

            Assert.Equal(endpointConfigObj, configuration.ResourceEndpoint);
        }

        [Fact]
        public void ConstructWithFrameworkProvider()
        {
            var frameworkProviderObj = FrameworkProviderMock.Object;
            
            var configuration = new GlimpseConfiguration(frameworkProviderObj, EndpointConfigMock.Object);

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
            var frameworkProviderObj = FrameworkProviderMock.Object;

            var configuration = new GlimpseConfiguration(frameworkProviderObj, EndpointConfigMock.Object);

            Assert.NotNull(configuration.PersistanceStore);
            FrameworkProviderMock.Verify(fp => fp.HttpServerStore, Times.AtLeastOnce());
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
            Assert.NotNull(Configuration.Validators);
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
            Assert.Equal(0, Configuration.Validators.Count);
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullEndpointConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseConfiguration(FrameworkProviderMock.Object, null));
        }

        [Fact]
        public void ThrowExceptionWhenConstructedWithNullFrameworkProvider()
        {
            Assert.Throws<ArgumentNullException>(()=>new GlimpseConfiguration(null, EndpointConfigMock.Object));
        }

        [Fact]
        public void FrameworkProviderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(()=>Configuration.FrameworkProvider = null);
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
        public void PipeliseInspectorsCannotBeNull()
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
            Assert.Throws<ArgumentNullException>(() => Configuration.Validators = null);
        }

        public void Dispose()
        {
            EndpointConfigMock = null;
            FrameworkProviderMock = null;
            Configuration = null;
        }
    }
}