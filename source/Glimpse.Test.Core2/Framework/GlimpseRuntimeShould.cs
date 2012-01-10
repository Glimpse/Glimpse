using System;
using System.Diagnostics;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Glimpse.Test.Core2.Framework
{
    public class GlimpseRuntimeShould : IDisposable
    {
        private GlimpseRuntimeTester tester;
        public GlimpseRuntimeTester Runtime
        {
            get { return tester ?? (tester = GlimpseRuntimeTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Runtime = null;
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
            Runtime.FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);

            Runtime.BeginRequest();

            Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.RequestIdKey, It.IsAny<Guid>()));
        }

        [Fact]
        public void StartGlobalStopwatchOnBeginRequest()
        {
            Runtime.FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);

            Runtime.BeginRequest();

            Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.GlobalStopwatchKey, It.IsAny<Stopwatch>()));
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
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithLifeCycleMismatch()
        {
            Runtime.TabMetadataMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.BeginRequest);

            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs(LifeCycleSupport.EndRequest);

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Never());
        }

        [Fact]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithUnknownRuntimeContextType()
        {
            Runtime.TabMetadataMock.Setup(m => m.RequestContextType).Returns<Type>(null);

            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithDuplicateCollectionEntries()
        {
            //Insert the same plugin multiple times
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.AtLeastOnce());
        }

        [Fact]
        public void ExecutePluginThatFails()
        {
            Runtime.TabMock.Setup(p => p.GetData(It.IsAny<IServiceLocator>())).Throws<DummyException>();

            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<IServiceLocator>()), Times.Once());
            //Make sure the excption type above is logged here.
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void ExecutePluginsWithEmptyCollection()
        {
            Runtime.Configuration.Tabs.Clear();

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, object>>(Constants.PluginResultsDataStoreKey);
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
            var setupMock = Runtime.TabMock.As<IGlimpseTabSetup>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => new DummyTab(), Runtime.TabMetadataMock.Object));

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
        }

        [Fact]
        public void InitializeWithSetupTabThatFails()
        {
            var setupMock = Runtime.TabMock.As<IGlimpseTabSetup>();
            setupMock.Setup(s => s.Setup()).Throws<DummyException>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => new DummyTab(), Runtime.TabMetadataMock.Object));

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void InitializeWithPipelineInspectors()
        {
            Runtime.Configuration.PipelineInspectors.Add(Runtime.PipelineInspectorMock.Object);

            Runtime.Initialize();

            Runtime.PipelineInspectorMock.Verify(pi => pi.Setup(), Times.Once());
        }

        [Fact]
        public void InitializeWithPipelineInspectorThatFails()
        {
            Runtime.PipelineInspectorMock.Setup(pi => pi.Setup()).Throws<DummyException>();

            Runtime.Configuration.PipelineInspectors.Add(Runtime.PipelineInspectorMock.Object);

            Runtime.Initialize();

            Runtime.PipelineInspectorMock.Verify(pi => pi.Setup(), Times.Once());
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.PipelineInspectors.Count));
        }

        [Fact]
        public void InjectHttpResponseBodyDuringEndRequest()
        {
            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.FrameworkProviderMock.Verify(fp => fp.InjectHttpResponseBody(It.IsAny<string>()));
        }
        
        [Fact]
        public void PersistDataDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Runtime.Configuration.Tabs.Count));
            Runtime.PersistanceStoreMock.Verify(ps => ps.Save(It.IsAny<GlimpseMetadata>()));
        }

        [Fact]
        public void SerializeDataDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Runtime.Configuration.Tabs.Count));
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
            Runtime.Configuration.Tabs.Add(new Lazy<IGlimpseTab, IGlimpseTabMetadata>(() => Runtime.TabMock.Object, Runtime.TabMetadataMock.Object));

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader(Constants.HttpHeader, It.IsAny<string>()));
        }

        [Fact]
        public void UpdateConfigurationWithChangeToAutoDiscovery()
        {
            Runtime.Configuration.Tabs.Discoverability.AutoDiscover = false; //being explicit for testing sake
            var runtime = Runtime; //force instantiation of Runtime property

            Assert.Equal(0, Runtime.Configuration.Tabs.Count);

            Runtime.Configuration.Tabs.Discoverability.AutoDiscover = true; //being explicit for testing sake
            runtime.UpdateConfiguration(Runtime.Configuration);

            Assert.True(Runtime.Configuration.Tabs.Count > 0);
        }

        [Fact]
        public void UpdateConfigurationWithoutAutoDiscovery()
        {
            Runtime.Configuration.Tabs.Discoverability.AutoDiscover = false; //being explicit for testing sake
            var runtime = Runtime; //force instantiation of Runtime property

            Assert.Equal(0, Runtime.Configuration.Tabs.Count);

            runtime.UpdateConfiguration(Runtime.Configuration);

            Assert.Equal(0, Runtime.Configuration.Tabs.Count);
        }

        [Fact]
        public void ExecuteResource()
        {
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IDictionary<string, string>>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name.ToLower());

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IDictionary<string, string>>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(Runtime.FrameworkProviderMock.Object));
        }

        [Fact]
        public void HandleUnknownResource()
        {
            Runtime.Configuration.Resources.Clear();

            Runtime.ExecuteResource("random name that doesn't exist");

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(404), Times.Once());
        }

        [Fact]
        public void HandleDuplicateResources()
        {
            var name = "Duplicate";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name);

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact]
        public void ThrowExceptionWithEmptyResourceName()
        {
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource(""));
        }

        [Fact]
        public void HandleResourcesThatThrowExceptions()
        {
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IDictionary<string, string>>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name);

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact]
        public void EnsureNullIsNotPassedToResourceExecute()
        {
            var name = "aName";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IDictionary<string, string>>())).Returns(
                Runtime.ResourceResultMock.Object);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, null);

            Runtime.ResourceMock.Verify(r => r.Execute(null), Times.Never());
        }

        [Fact]
        public void HandleResourceResultsThatThrowExceptions()
        {
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IDictionary<string, string>>())).Returns(Runtime.ResourceResultMock.Object);

            Runtime.ResourceResultMock.Setup(rr => rr.Execute(It.IsAny<IFrameworkProvider>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name);

            Runtime.LoggerMock.Verify(l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public void ProvideEnabledInfoOnInitializing()
        {
            Runtime.Configuration.RuntimePolicies.Add(
                new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(() => Runtime.ValidatorMock.Object,
                                                                       new RuntimePolicyAttribute()));
            ;

            var result = Runtime.Initialize();

            Assert.True(result);
        }

        [Fact]
        public void ProvideLowestModeLevelOnInitializing()
        {
            var offValidatorMock = new Mock<IRuntimePolicy>();
            offValidatorMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.Off);

            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>Runtime.ValidatorMock.Object, new RuntimePolicyAttribute()));
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>offValidatorMock.Object, new RuntimePolicyAttribute()));

            var result = Runtime.Initialize();

            Assert.False(result);
        }

        [Fact]
        public void NotIncreaseModeOverLifetimeOfRequest()
        {
            var glimpseMode = RuntimePolicy.ModifyResponseBody;
            Runtime.Configuration.BasePolicy = glimpseMode;

            var firstMode = Runtime.Initialize();

            Assert.True(firstMode);

            Runtime.Configuration.BasePolicy = RuntimePolicy.On;
            Runtime.UpdateConfiguration(Runtime.Configuration);

            Runtime.BeginRequest();

            Assert.Equal(glimpseMode, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void RespectConfigurationSettingInValidators()
        {
            Runtime.Configuration.BasePolicy = RuntimePolicy.Off;

            Runtime.ValidatorMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.On);
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>Runtime.ValidatorMock.Object,new RuntimePolicyAttribute()));

            var result = Runtime.Initialize();

            Assert.False(result);
        }

        [Fact]
        public void ValidateAtBeginRequest()
        {
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>Runtime.ValidatorMock.Object,new RuntimePolicyAttribute()));

            Runtime.BeginRequest();

            Runtime.ValidatorMock.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.AtLeastOnce());
        }

        [Fact]
        public void SkipEecutingBeginRequestIfGlimpseModeIfOff()
        {
            Runtime.Configuration.BasePolicy = RuntimePolicy.Off;

            Runtime.BeginRequest();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact] //False result means GlimpseMode == Off
        public void WriteCurrentModeToRequestState()
        {
            Runtime.ValidatorMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.ModifyResponseBody);
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>Runtime.ValidatorMock.Object, new RuntimePolicyAttribute()));

            var result = Runtime.Initialize();

            Assert.True(result);

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void SkipExecutingTabsIfGlipseModeIsOff()
        {
            Runtime.Configuration.BasePolicy = RuntimePolicy.Off;

            Runtime.ExecuteTabs();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void SkipExecutingResourceIfGlipseModeIsOff()
        {
            Runtime.Configuration.BasePolicy = RuntimePolicy.Off;

            Runtime.ExecuteResource("doesn't matter");

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void ValidateAtEndRequest()
        {
            Runtime.Configuration.BasePolicy = RuntimePolicy.Off;

            Runtime.EndRequest();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void ExecuteOnlyTheProperValidators()
        {
            var validatorMock2 = new Mock<IRuntimePolicy>();
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>Runtime.ValidatorMock.Object, new RuntimePolicyAttribute(RuntimeEvent.Initialize)));
            Runtime.Configuration.RuntimePolicies.Add(new Lazy<IRuntimePolicy, IRuntimePolicyMetadata>(()=>validatorMock2.Object, new RuntimePolicyAttribute(RuntimeEvent.EndRequest)));

            Runtime.Initialize();

            Runtime.ValidatorMock.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.Once());
            validatorMock2.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.Never());
        }

        [Fact]
        public void SetIsInitializedWhenInitialized()
        {
            Runtime.Initialize();

            Assert.True(Runtime.IsInitialized);
        }
    }
}