using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseConfigurationShould
    {
        private GlimpseConfiguration Configuration { get; set; }

        public GlimpseConfigurationShould()
        {
            var framworkProviderMock = new Mock<IFrameworkProvider>().Setup();
            Configuration = new GlimpseConfiguration(framworkProviderMock.Object);
        }

        [Fact]
        public void ConstructWithFrameworkProvider()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var frameworkProviderObject = frameworkProviderMock.Object;

            var config = new GlimpseConfiguration(frameworkProviderObject);

            Assert.Equal(frameworkProviderObject, config.FrameworkProvider);
            Assert.NotNull(config.Serializer as JsonNetSerializer);
            Assert.NotNull(config.Plugins);
            Assert.NotNull(config.PipelineModifiers);
        }

        [Fact(Skip = "This needs to be fixed - looking into code contracts")]
        public void ThrowExceptionWhenConstructedWithoutFrameworkProvider()
        {
            new GlimpseConfiguration(null);
        }
    }
}