using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseConfigurationShould
    {
        [Fact]
        public void ConstructWithRuntimeServicesFactory()
        {
            var runtimeServiceMock = new Mock<IRuntimeService>();
            var runtimeService = runtimeServiceMock.Object;

            var config = new GlimpseConfiguration(runtimeService);

            Assert.Equal(runtimeService, config.RuntimeService);
        }


        [Fact(Skip="Come back to plugin discovery")]
        public void DiscoverPlugins()
        {
            //var glimpseConfiguration = new GlimpseConfiguration();
            //glimpseConfiguration.Plugins.Discover();
        }
    }
}
