using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseServiceLocatorShould
    {
        [Fact]
        public void ConstructWithRequestContextAndPluginStoreSet()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var anyObject = new {Any="object"};
            var pluginStore = dataStoreMock.Object;
            var locator = new GlimpseServiceLocator(anyObject,pluginStore, new GlimpseCollection<IGlimpsePipelineModifier>());

            Assert.NotNull(locator);
            Assert.True(typeof(IServiceLocator).IsInstanceOfType(locator));
            Assert.Equal(anyObject, locator.RequestContext);
            Assert.Equal(pluginStore, locator.PluginStore);
        }

        [Fact]
        public void GetPipelineModifier()
        {
            var dataStoreMock = new Mock<IDataStore>();
            var anyObject = new { Any = "object" };
            var pluginStore = dataStoreMock.Object;
            var locator = new GlimpseServiceLocator(anyObject, pluginStore, new GlimpseCollection<IGlimpsePipelineModifier>
                                                                                {
                                                                                    new TestGlimpsePipelineModifier()
                                                                                });

            var testGlimpsePipelineModifier = locator.GetPipelineModifier<TestGlimpsePipelineModifier>();
            Assert.NotNull(testGlimpsePipelineModifier);
            Assert.IsType<TestGlimpsePipelineModifier>(testGlimpsePipelineModifier);


        }
    }
}
