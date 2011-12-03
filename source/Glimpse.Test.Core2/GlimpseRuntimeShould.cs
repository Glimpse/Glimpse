using System;
using System.Diagnostics;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;
using System.Collections.Generic;

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
        public void Initialize()
        {
            var pipelineModifierMock = new Mock<IGlimpsePipelineModifier>();
            pipelineModifierMock.Setup(pm => pm.Setup()).Throws<Exception>();

            var pluginMock = new Mock<IGlimpseTab>();
            var setupMock = pluginMock.As<IGlimpsePluginSetup>();
            setupMock.Setup(s => s.Setup()).Throws<Exception>();

            Configuration.PipelineModifiers.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.PipelineModifiers.Add(pipelineModifierMock.Object);
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(()=>pluginMock.Object,new GlimpseTabAttribute()));

            var runtime = new GlimpseRuntime(Configuration);

            runtime.Initialize();

            pipelineModifierMock.Verify(pm=>pm.Setup(), Times.Once());
            setupMock.Verify(pm=>pm.Setup(), Times.Once());
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

        [Fact]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof (object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(()=>pluginMock.Object, metadataMock.Object));

            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();

            pluginMock.Verify(p=>p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void NotExecutePluginsWithoutMatchingRuntimeContextType()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(string));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();

            pluginMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Never());
        }

        [Fact]
        public void ExecutePluginsBeforeBeginRequestThrowsException()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(string));
            var pluginMock = new Mock<IGlimpseTab>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var runtime = new GlimpseRuntime(Configuration);

            //runtime.BeginRequest(); commented out on purpose for this test

            Assert.Throws<Exception>(()=>runtime.ExecutePlugins());
        }

        [Fact]
        public void ExecutePluginsWithDefaultLifeCycle()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();
            pluginMock.Setup(p => p.GetData(It.IsAny<IServiceLocator>())).Returns("a result");

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>("__GlimpseResults");
            Assert.NotNull(results);
            Assert.True(results.Count > 0);

            pluginMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void FlagsTest()
        {
            var support = LifeCycleSupport.EndRequest;

            Assert.True(support.HasFlag(LifeCycleSupport.EndRequest), "End is End");

            support = LifeCycleSupport.EndRequest | LifeCycleSupport.SessionAccessEnd;

            Assert.True(support.HasFlag(LifeCycleSupport.EndRequest), "End in End|SessionEnd");
            Assert.True(support.HasFlag(LifeCycleSupport.SessionAccessEnd), "SessionEnd in End|SessionEnd");
            //support End OR Begin
            Assert.True(support.HasFlag(LifeCycleSupport.EndRequest & LifeCycleSupport.BeginRequest), "End|Begin in End|SessionEnd");
            //support End AND SessionEnd
            Assert.True(support.HasFlag(LifeCycleSupport.EndRequest | LifeCycleSupport.SessionAccessEnd), "End|SessionEnd in End|SessionEnd");
            Assert.False(support.HasFlag(LifeCycleSupport.EndRequest | LifeCycleSupport.BeginRequest), "End|Begin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(LifeCycleSupport.BeginRequest), "Begin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(LifeCycleSupport.SessionAccessBegin), "SessionBegin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(LifeCycleSupport.BeginRequest | LifeCycleSupport.SessionAccessBegin), "Begin|SessionBegin NOT in End|SessionEnd");
        }

        [Fact]
        public void ExecutePluginThatFails()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();
            pluginMock.Setup(p => p.GetData(It.IsAny<IServiceLocator>())).Throws<Exception>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>("__GlimpseResults");
            Assert.NotNull(results);
            Assert.True(results.Count == 0);
            

            pluginMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());   
        }

        [Fact]
        public void SerializeDataDuringEndRequest()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));


            var serializerMock = new Mock<IGlimpseSerializer>();
            Configuration.Serializer = serializerMock.Object;
            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();
            runtime.EndRequest();

            serializerMock.Verify(s=>s.Serialize(It.IsAny<object>()), Times.Exactly(Configuration.Plugins.Count));
        }

        [Fact]
        public void PersistDataDuringEndRequest()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();

            Configuration.Plugins.Discoverability.AutoDiscover = false;
            Configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var serializerMock = new Mock<IGlimpseSerializer>();
            Configuration.Serializer = serializerMock.Object;

            var persistanceStoreMock = new Mock<IGlimpsePersistanceStore>();
            Configuration.PersistanceStore = persistanceStoreMock.Object;
            
            var runtime = new GlimpseRuntime(Configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();
            runtime.EndRequest();

            serializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Configuration.Plugins.Count));
            persistanceStoreMock.Verify(ps=>ps.Save(It.IsAny<GlimpseMetadata>()));
        }

        [Fact]
        public void SetResponseHeaderDuringEndRequest()
        {
            var metadataMock = new Mock<IGlimpsePluginMetadata>();
            metadataMock.Setup(m => m.RequestContextType).Returns(typeof(object));
            metadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.EndRequest);
            var pluginMock = new Mock<IGlimpseTab>();

            var frameworkProviderMock = new Mock<IFrameworkProvider>().Setup();
            var configuration = new GlimpseConfiguration(frameworkProviderMock.Object);

            configuration.Plugins.Discoverability.AutoDiscover = false;
            configuration.Plugins.Add(new Lazy<IGlimpseTab, IGlimpsePluginMetadata>(() => pluginMock.Object, metadataMock.Object));

            var serializerMock = new Mock<IGlimpseSerializer>();
            configuration.Serializer = serializerMock.Object;

            var persistanceStoreMock = new Mock<IGlimpsePersistanceStore>();
            configuration.PersistanceStore = persistanceStoreMock.Object;
            
            var runtime = new GlimpseRuntime(configuration);

            runtime.BeginRequest();
            runtime.ExecutePlugins();
            runtime.EndRequest();

            frameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader("X-Glimpse-RequestID", It.IsAny<string>()));
        }

        //End request
        //serialize data
        //persist data
        //try to add headers
        //try to add script tags
    }
}
