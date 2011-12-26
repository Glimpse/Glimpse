using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseConfigurationShould
    {
        [Fact]
        public void ConstructWithFrameworkProvider()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            frameworkProviderMock.Setup(fp => fp.HttpServerStore).Returns(new DictionaryDataStoreAdapter(new Dictionary<string,object>()));
            var frameworkProviderObject = frameworkProviderMock.Object;

            var endpointConfig = new Mock<IGlimpseResourceEndpointConfiguration>();

            var config = new GlimpseConfiguration(frameworkProviderObject, endpointConfig.Object);

            Assert.Equal(frameworkProviderObject, config.FrameworkProvider);
            Assert.NotNull(config.Serializer as JsonNetSerializer);
            Assert.NotNull(config.Plugins);
            Assert.NotNull(config.PipelineModifiers);
            Assert.NotNull(config.PersistanceStore);
            Assert.NotNull(config.Resources);
            Assert.NotNull(config.HtmlEncoder);
            Assert.NotNull(config.Validators);
        }

        [Fact(Skip = "This needs to be fixed - looking into code contracts")]
        public void ThrowExceptionWhenConstructedWithoutFrameworkProvider()
        {
            new GlimpseConfiguration(null, null);
        }
    }
}