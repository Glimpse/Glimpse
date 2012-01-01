using System;
using System.Diagnostics;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Extensions;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Glimpse.Test.Core2
{
    public class GlimpseRuntimeShould : IDisposable
    {
        public GlimpseRuntimeShould()
        {
            FrameworkProviderMock = new Mock<IFrameworkProvider>().Setup();
            EndpointConfigMock = new Mock<IGlimpseResourceEndpointConfiguration>();
            HttpRequestStoreMock = new Mock<IDataStore>();
            TabMetadataMock = new Mock<IGlimpseTabMetadata>().Setup();
            TabMock = new Mock<IGlimpseTab>().Setup();
            PipelineInspectorMock = new Mock<IGlimpsePipelineInspector>();
            SerializerMock = new Mock<IGlimpseSerializer>();
            PersistanceStoreMock = new Mock<IGlimpsePersistanceStore>();
        }

        private GlimpseConfiguration configuration;
        private GlimpseConfiguration Configuration
        {
            get
            {
                if (configuration != null) return configuration;

                configuration =
                    new GlimpseConfiguration(FrameworkProviderMock.Object, EndpointConfigMock.Object).
                        TurnOffAutoDiscover();
                configuration.Serializer = SerializerMock.Object;
                configuration.PersistanceStore = PersistanceStoreMock.Object;

                return configuration;
            }
            set { configuration = value; }
        }

        private Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
        private Mock<IGlimpseResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        private Mock<IDataStore> HttpRequestStoreMock { get; set; }
        private Mock<IGlimpseTabMetadata> TabMetadataMock { get; set; }
        private Mock<IGlimpseTab> TabMock { get; set; }
        private Mock<IGlimpsePipelineInspector> PipelineInspectorMock { get; set; }
        private Mock<IGlimpseSerializer> SerializerMock { get; set; }
        private Mock<IGlimpsePersistanceStore> PersistanceStoreMock { get; set;}

        private GlimpseRuntime runtime;
        private GlimpseRuntime Runtime
        {
            get { return runtime ?? (runtime = new GlimpseRuntime(Configuration)); }
            set { runtime = value; }
        }


        [Fact]
        public void CreatesServiceLocatorOnBeginRequest()
        {
            Runtime.BeginRequest();

            Assert.NotNull(Runtime.ServiceLocator);
        }

        [Fact]
        public void SetRequestIdOnBeginRequest()
        {
            FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(HttpRequestStoreMock.Object);

            var runtime =
                new GlimpseRuntime(new GlimpseConfiguration(FrameworkProviderMock.Object, EndpointConfigMock.Object));

            runtime.BeginRequest();

            HttpRequestStoreMock.Verify(store => store.Set(Constants.RequestIdKey, It.IsAny<Guid>()));
        }

        [Fact]
        public void StartGlobalStopwatchOnBeginRequest()
        {
            FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(HttpRequestStoreMock.Object);

            var runtime =
                new GlimpseRuntime(new GlimpseConfiguration(FrameworkProviderMock.Object, EndpointConfigMock.Object));

            runtime.BeginRequest();

            HttpRequestStoreMock.Verify(store => store.Set(Constants.GlobalStopwatchKey, It.IsAny<Stopwatch>()));
        }

        [Fact]
        public void Construct()
        {
            Assert.False(string.IsNullOrWhiteSpace(Runtime.Version));
        }

        [Fact]
        public void ThrowsExceptionIfEndRequestIsCalledBeforeBeginRequest()
        {
            //runtime.BeginRequest(); commented out on purpose for this test

            Assert.Throws<MethodAccessException>(() => Runtime.EndRequest());
        }

        [Fact]
        public void ExecutePluginsWithDefaultLifeCycle()
        {
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            
            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithLifeCycleMismatch()
        {
            TabMetadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.BeginRequest);

            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins(LifeCycleSupport.EndRequest);

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Never());
        }

        [Fact]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithUnknownRuntimeContextType()
        {
            TabMetadataMock.Setup(m => m.RequestContextType).Returns<Type>(null);

            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithDuplicateCollectionEntries()
        {
            //Insert the same plugin multiple times
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.AtLeastOnce());
        }

        [Fact(Skip = "Need to add logging, and verify the logger is called")]
        public void ExecutePluginThatFails()
        {
            TabMock.Setup(p => p.GetData(It.IsAny<IServiceLocator>())).Throws<Exception>();

            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithEmptyCollection()
        {
            Configuration.Tabs.Clear();

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();

            var results = Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void FlagsTest()
        {
            //This test is just to help me keep my sanity with bitwise operators
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
        public void HaveASemanticVersion()
        {
            Version version;
            Assert.True(Version.TryParse(Runtime.Version, out version));
            Assert.NotNull(version.Major);
            Assert.NotNull(version.Minor);
            Assert.NotNull(version.Build);
            Assert.Equal(-1, version.Revision);
        }

        [Fact]
        public void InitializeWithSetupTabs()
        {
            var setupMock = TabMock.As<IGlimpseTabSetup>();

            //one tab needs setup, the other does not
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => new DummyTab(), TabMetadataMock.Object));

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
        }

        [Fact(Skip = "Need to add logging, and verify the logger is called")]
        public void InitializeWithSetupTabThatFails()
        {
            var setupMock = TabMock.As<IGlimpseTabSetup>();
            setupMock.Setup(s => s.Setup()).Throws<Exception>();

            //one tab needs setup, the other does not
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => new DummyTab(), TabMetadataMock.Object));

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
        }

        [Fact]
        public void InitializeWithPipelineInspectors()
        {
            Configuration.PipelineInspectors.Add(PipelineInspectorMock.Object);

            Runtime.Initialize();

            PipelineInspectorMock.Verify(pi=>pi.Setup(), Times.Once());
        }

        [Fact(Skip = "Need to add logging, and verify the logger is called")]
        public void InitializeWithPipelineInspectorThatFails()
        {
            PipelineInspectorMock.Setup(pi => pi.Setup()).Throws<Exception>();

            Configuration.PipelineInspectors.Add(PipelineInspectorMock.Object);

            Runtime.Initialize();

            PipelineInspectorMock.Verify(pi => pi.Setup(), Times.Once());
        }

        [Fact]
        public void InjectHttpResponseBodyDuringEndRequest()
        {
            Runtime.BeginRequest();
            Runtime.ExecutePlugins();
            Runtime.EndRequest();

            FrameworkProviderMock.Verify(fp => fp.InjectHttpResponseBody(It.IsAny<string>()));
        }

        
        [Fact]
        public void PersistDataDuringEndRequest()
        {
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();
            Runtime.EndRequest();

            SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Configuration.Tabs.Count));
            PersistanceStoreMock.Verify(ps => ps.Save(It.IsAny<GlimpseMetadata>()));
        }

        [Fact]
        public void SerializeDataDuringEndRequest()
        {
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();
            Runtime.EndRequest();

            SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Configuration.Tabs.Count));
        }

        [Fact]
        public void ServiceLocatorShouldThrowIfRequestHasNotBegun()
        {
            Assert.Throws<MethodAccessException>(() => { var serviceLocator = Runtime.ServiceLocator; });

            Runtime.BeginRequest();
            Assert.NotNull(Runtime.ServiceLocator);
        }

        [Fact]
        public void SetResponseHeaderDuringEndRequest()
        {
            Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => TabMock.Object, TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecutePlugins();
            Runtime.EndRequest();

            FrameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader(Constants.HttpHeader, It.IsAny<string>()));
        }

        [Fact]
        public void UpdateConfigurationWithAutoDiscovery()
        {

            Configuration.Tabs.Discoverability.AutoDiscover = true; //being explicit for testing sake

            var runtime = Runtime; //force instantiation of Runtime property

            var tabCount = Configuration.Tabs.Count;
            Assert.True(tabCount > 0);

            runtime.UpdateConfiguration(Configuration);

            Assert.Equal(tabCount, Configuration.Tabs.Count);
        }

        [Fact]
        public void UpdateConfigurationWithChangeToAutoDiscovery()
        {
            Configuration.Tabs.Discoverability.AutoDiscover = false; //being explicit for testing sake
            var runtime = Runtime; //force instantiation of Runtime property

            Assert.Equal(0, Configuration.Tabs.Count);

            Configuration.Tabs.Discoverability.AutoDiscover = true; //being explicit for testing sake
            runtime.UpdateConfiguration(Configuration);

            Assert.True(Configuration.Tabs.Count > 0);
        }

        [Fact]
        public void UpdateConfigurationWithoutAutoDiscovery()
        {
            Configuration.Tabs.Discoverability.AutoDiscover = false; //being explicit for testing sake
            var runtime = Runtime; //force instantiation of Runtime property

            Assert.Equal(0, Configuration.Tabs.Count);

            runtime.UpdateConfiguration(Configuration);

            Assert.Equal(0, Configuration.Tabs.Count);
        }

        public void Dispose()
        {
            Configuration = null;
            FrameworkProviderMock = null;
            EndpointConfigMock = null;
            HttpRequestStoreMock = null;
            TabMetadataMock = null;
            TabMock = null;
            Runtime = null;
            PipelineInspectorMock = null;
            SerializerMock = null;
            PersistanceStoreMock = null;
        }
    }
}