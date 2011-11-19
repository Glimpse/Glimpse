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

            var config = new GlimpseConfiguration(frameworkProviderObject);

            Assert.Equal(frameworkProviderObject, config.FrameworkProvider);
            Assert.NotNull(config.Serializer as JsonNetSerializer);
            Assert.NotNull(config.Plugins);
            Assert.NotNull(config.PipelineModifiers);
            Assert.NotNull(config.PersistanceStore);
        }

        [Fact(Skip = "This needs to be fixed - looking into code contracts")]
        public void ThrowExceptionWhenConstructedWithoutFrameworkProvider()
        {
            new GlimpseConfiguration(null);
        }
    }
}