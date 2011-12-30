using System;
using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseConfigurationShould
    {

        public GlimpseConfigurationShould()
        {
            FrameworkProviderMock = new Mock<IFrameworkProvider>();
            FrameworkProviderMock.Setup(fp => fp.HttpServerStore).Returns(new DictionaryDataStoreAdapter(new Dictionary<string, object>()));

            EndpointConfigMock = new Mock<IGlimpseResourceEndpointConfiguration>();
        }



        private GlimpseConfiguration Configuration
        {
            get { return new GlimpseConfiguration(FrameworkProviderMock.Object, EndpointConfigMock.Object); }
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
    }
}