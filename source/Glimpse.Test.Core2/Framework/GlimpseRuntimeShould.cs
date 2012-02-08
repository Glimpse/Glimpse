using System;
using System.Diagnostics;
using System.Linq;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Glimpse.Test.Core2.Tester;
using Moq;
using Moq.Protected;
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
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithLifeCycleMismatch()
        {
            Runtime.TabMock.Setup(m => m.LifeCycleSupport).Returns(LifeCycleSupport.BeginRequest);

            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs(LifeCycleSupport.EndRequest);

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithUnknownRuntimeContextType()
        {
            Runtime.TabMock.Setup(m => m.RequestContextType).Returns<Type>(null);

            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithDuplicateCollectionEntries()
        {
            //Insert the same plugin multiple times
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.AtLeastOnce());
        }

        [Fact]
        public void ExecutePluginThatFails()
        {
            Runtime.TabMock.Setup(p => p.GetData(It.IsAny<ITabContext>())).Throws<DummyException>();

            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
            //Make sure the excption type above is logged here.
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void ExecutePluginsWithEmptyCollection()
        {
            Runtime.Configuration.Tabs.Clear();

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
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
            var setupMock = Runtime.TabMock.As<ISetup>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
        }

        [Fact]
        public void InitializeWithSetupTabThatFails()
        {
            var setupMock = Runtime.TabMock.As<ISetup>();
            setupMock.Setup(s => s.Setup()).Throws<DummyException>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(), Times.Once());
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void InitializeWithPipelineInspectors()
        {
            Runtime.Configuration.PipelineInspectors.Add(Runtime.PipelineInspectorMock.Object);

            Runtime.Initialize();

            Runtime.PipelineInspectorMock.Verify(pi => pi.Setup(It.IsAny<IPipelineInspectorContext>()), Times.Once());
        }

        [Fact]
        public void InitializeWithPipelineInspectorThatFails()
        {
            Runtime.PipelineInspectorMock.Setup(pi => pi.Setup(It.IsAny<IPipelineInspectorContext>())).Throws<DummyException>();

            Runtime.Configuration.PipelineInspectors.Add(Runtime.PipelineInspectorMock.Object);

            Runtime.Initialize();

            Runtime.PipelineInspectorMock.Verify(pi => pi.Setup(It.IsAny<IPipelineInspectorContext>()), Times.Once());
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
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Runtime.Configuration.Tabs.Count));
            Runtime.PersistanceStoreMock.Verify(ps => ps.Save(It.IsAny<GlimpseMetadata>()));
        }

        [Fact]
        public void SerializeDataDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.SerializerMock.Verify(s => s.Serialize(It.IsAny<object>()), Times.Exactly(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void SetResponseHeaderDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest();
            Runtime.ExecuteTabs();
            Runtime.EndRequest();

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseHeader(Constants.HttpResponseHeader, It.IsAny<string>()));
        }


        [Fact]
        public void ExecuteDefaultResource()
        {
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.DefaultResource = Runtime.ResourceMock.Object;

            Runtime.ExecuteDefaultResource();

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact]
        public void ExecuteResourceWithOrderedParameters()
        {
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name.ToLower(), new[]{"One","Two"});

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact]
        public void ExecuteResourceWithNamedParameters()
        {
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name.ToLower(), new Dictionary<string, string>{{"One", "1"}, {"Two","2"}});

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact]
        public void HandleUnknownResource()
        {
            Runtime.Configuration.Resources.Clear();

            Runtime.ExecuteResource("random name that doesn't exist", new string[]{});

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(404), Times.Once());
        }

        [Fact]
        public void HandleDuplicateResources()
        {
            var name = "Duplicate";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new string[]{});

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact]
        public void ThrowExceptionWithEmptyResourceName()
        {
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource("", new string[]{}));
        }

        [Fact]
        public void HandleResourcesThatThrowExceptions()
        {
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new string[]{});

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact]
        public void EnsureNullIsNotPassedToResourceExecute()
        {
            var name = "aName";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(
                Runtime.ResourceResultMock.Object);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new Dictionary<string, string>());

            Runtime.ResourceMock.Verify(r => r.Execute(null), Times.Never());
        }

        [Fact]
        public void HandleResourceResultsThatThrowExceptions()
        {
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);

            Runtime.ResourceResultMock.Setup(rr => rr.Execute(It.IsAny<IResourceResultContext>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new string[]{});

            Runtime.LoggerMock.Verify(l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once());
        }

        [Fact]
        public void ProvideEnabledInfoOnInitializing()
        {
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            var result = Runtime.Initialize();

            Assert.True(result);
        }

        [Fact]
        public void ProvideLowestModeLevelOnInitializing()
        {
            var offValidatorMock = new Mock<IRuntimePolicy>();
            offValidatorMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.Off);
            offValidatorMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.All);

            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);
            Runtime.Configuration.RuntimePolicies.Add(offValidatorMock.Object);

            var result = Runtime.Initialize();

            Assert.False(result);
        }

        [Fact]
        public void NotIncreaseModeOverLifetimeOfRequest()
        {
            var glimpseMode = RuntimePolicy.ModifyResponseBody;
            Runtime.Configuration.BaseRuntimePolicy = glimpseMode;

            var firstMode = Runtime.Initialize();

            Assert.True(firstMode);

            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.On;

            Runtime.BeginRequest();

            Assert.Equal(glimpseMode, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void RespectConfigurationSettingInValidators()
        {
            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.Off;

            Runtime.RuntimePolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.On);
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            var result = Runtime.Initialize();

            Assert.False(result);
        }

        [Fact]
        public void ValidateAtBeginRequest()
        {
            Runtime.RuntimePolicyMock.Setup(rp => rp.ExecuteOn).Returns(RuntimeEvent.BeginRequest);

            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            Runtime.BeginRequest();

            Runtime.RuntimePolicyMock.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.AtLeastOnce());
        }

        [Fact]
        public void SkipEecutingBeginRequestIfGlimpseModeIfOff()
        {
            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.Off;

            Runtime.BeginRequest();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact] //False result means GlimpseMode == Off
        public void WriteCurrentModeToRequestState()
        {
            Runtime.RuntimePolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.ModifyResponseBody);
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            var result = Runtime.Initialize();

            Assert.True(result);

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void SkipExecutingTabsIfGlipseModeIsOff()
        {
            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.Off;

            Runtime.ExecuteTabs();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void SkipExecutingResourceIfGlipseModeIsOff()
        {
            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.Off;

            Runtime.ExecuteResource("doesn't matter", new string[]{});

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void ValidateAtEndRequest()
        {
            Runtime.Configuration.BaseRuntimePolicy = RuntimePolicy.Off;

            Runtime.EndRequest();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePermissionsKey));
        }

        [Fact]
        public void ExecuteOnlyTheProperValidators()
        {
            var validatorMock2 = new Mock<IRuntimePolicy>();
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);
            Runtime.Configuration.RuntimePolicies.Add(validatorMock2.Object);

            Runtime.Initialize();

            Runtime.RuntimePolicyMock.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.Once());
            validatorMock2.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.Never());
        }

        [Fact]
        public void SetIsInitializedWhenInitialized()
        {
            Runtime.Initialize();

            Assert.True(Runtime.IsInitialized);
        }

        [Fact]
        public void GenerateNoScriptTagsWithoutClientScripts()
        {
            Assert.Equal("", Runtime.GenerateScriptTags(Guid.NewGuid()));
            
            Runtime.LoggerMock.Verify(l=>l.Warn(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void GenerateNoScriptTagsAndWarnWithOnlyIClientScriptImplementations()
        {
            var clientScriptMock = new Mock<IClientScript>();
            clientScriptMock.Setup(cs => cs.Order).Returns(ScriptOrder.ClientInterfaceScript);

            Runtime.Configuration.ClientScripts.Add(clientScriptMock.Object);

            Assert.Equal("", Runtime.GenerateScriptTags(Guid.NewGuid()));
            
            Runtime.LoggerMock.Verify(l => l.Warn(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagWithOneStaticResource()
        {
            var uri = "http://localhost/static";
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(Runtime.Version)).Returns(uri);
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode(uri)).Returns(uri + "/encoded");

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);

            var result = Runtime.GenerateScriptTags(Guid.NewGuid());

            Assert.Contains(uri, result);
        }

        [Fact]
        public void GenerateScriptTagsInOrder()
        {
            var callCount = 0;
            //Lightweight call sequence checking idea from http://dpwhelan.com/blog/software-development/moq-sequences/
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns("http://localhost/dynamic").Callback(()=>Assert.Equal(callCount++, 0));
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(Runtime.Version)).Returns("http://localhost/static").Callback(()=>Assert.Equal(callCount++, 1));
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("http://localhost/static")).Returns("http://localhost/static/encoded");

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.NotEmpty(Runtime.GenerateScriptTags(Guid.NewGuid()));
        }

        [Fact]
        public void GenerateScriptTagsWithDynamicScriptAndMatchingResource()
        {
            var resourceName = "resourceName";
            var uri = "http://somethingEncoded";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(resourceName);
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns(resourceName);
            Runtime.EndpointConfigMock.Protected().Setup<string>("GenerateUri", resourceName, ItExpr.IsAny<IEnumerable<KeyValuePair<string,string>>>(), ItExpr.IsAny<ILogger>()).Returns("http://something");
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("http://something")).Returns(uri);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Contains(uri, Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.ResourceMock.Verify(rm=>rm.Name, Times.AtLeastOnce());
            Runtime.EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), resourceName, ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(), ItExpr.IsAny<ILogger>());
            Runtime.EncoderMock.Verify(e => e.HtmlAttributeEncode("http://something"), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenEndpointConfigReturnsEmptyString()
        {
            var resourceName = "resourceName";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(resourceName);
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns(resourceName);
            Runtime.EndpointConfigMock.Protected().Setup<string>("GenerateUri", resourceName, ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(), ItExpr.IsAny<ILogger>()).Returns("");
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("")).Returns("");

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.ResourceMock.Verify(rm => rm.Name, Times.AtLeastOnce());
            Runtime.EndpointConfigMock.Protected().Verify<string>("GenerateUri", Times.Once(), resourceName, ItExpr.IsAny<IEnumerable<KeyValuePair<string, string>>>(), ItExpr.IsAny<ILogger>());
            Runtime.EncoderMock.Verify(e => e.HtmlAttributeEncode(""), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenMatchingResourceNotFound()
        {
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns("resourceName");

            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.LoggerMock.Verify(l=>l.Warn(It.IsAny<string>()));
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenStaticScriptReturnsEmptyString()
        {
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(Runtime.Version)).Returns("");

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));
        }
    }
}