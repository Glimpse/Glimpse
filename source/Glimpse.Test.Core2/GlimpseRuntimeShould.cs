using System;
using System.Diagnostics;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseRuntimeShould
    {
        public GlimpseConfiguration Configuration { get; set; }

        public GlimpseRuntimeShould()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>().Setup();

            Configuration = new GlimpseConfiguration(frameworkProviderMock.Object);
        }

        [Fact]
        public void Construct()
        {
            var runtime = new GlimpseRuntime(Configuration);

            Assert.NotNull(runtime);
        }

        [Fact]
        public void UpdateConfigurationWithAutoDiscovery()
        {
            var runtime = new GlimpseRuntime(Configuration);

            Assert.Equal(1, Configuration.Plugins.Count);

            runtime.UpdateConfiguration(Configuration);

            Assert.Equal(1, Configuration.Plugins.Count);
        }

        [Fact]
        public void UpdateConfigurationWithoutAutoDiscovery()
        {
            Configuration.Plugins.Discoverability.AutoDiscover = false;
            var runtime = new GlimpseRuntime(Configuration);

            Assert.Equal(0, Configuration.Plugins.Count);

            runtime.UpdateConfiguration(Configuration);

            Assert.Equal(0, Configuration.Plugins.Count);
        }

        [Fact]
        public void UpdateConfigurationWithChangeToAutoDiscovery()
        {
            Configuration.Plugins.Discoverability.AutoDiscover = false;
            var runtime = new GlimpseRuntime(Configuration);

            Assert.Equal(0, Configuration.Plugins.Count);

            Configuration.Plugins.Discoverability.AutoDiscover = true;
            runtime.UpdateConfiguration(Configuration);

            Assert.Equal(1, Configuration.Plugins.Count);
        }

        [Fact]
        public void BeginRequestCreatesServiceLocator()
        {
            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();

            var locator = runtime.ServiceLocator;

            Assert.NotNull(locator);
            Assert.NotNull(locator.RequestContext);
            Assert.NotNull(locator.PluginStore);
            Assert.NotNull(locator.GetPipelineModifier<TestGlimpsePipelineModifier>());
        }

        [Fact]
        public void BeginRequestStartsGlobalStopwatchAndSetsRequestId()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>().Setup();
            var dataStoreMock = new Mock<IDataStore>();
            frameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(dataStoreMock.Object);

            var runtime = new GlimpseRuntime(new GlimpseConfiguration(frameworkProviderMock.Object));

            runtime.BeginRequest();

            dataStoreMock.Verify(store => store.Set(It.IsAny<Stopwatch>()));
            dataStoreMock.Verify(store => store.Set(It.IsAny<Guid>()));
        }

        [Fact]
        public void ServiceLocatorShouldThrowIfRequestHasNotBegun()
        {
            var runtime = new GlimpseRuntime(Configuration);

            Assert.Throws<Exception>(()=>
                                         {
                                             var serviceLocator = runtime.ServiceLocator;
                                         });

            runtime.BeginRequest();
            Assert.NotNull(runtime.ServiceLocator);
        }
    }
}
