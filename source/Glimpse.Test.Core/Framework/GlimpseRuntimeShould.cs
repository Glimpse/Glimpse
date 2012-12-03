using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.TestDoubles;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
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

            Runtime.Initialize();
            Runtime.BeginRequest();

            Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.RequestIdKey, It.IsAny<Guid>()));
        }

        [Fact]
        public void StartGlobalStopwatchOnBeginRequest()
        {
            Runtime.FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();

            Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.GlobalStopwatchKey, It.IsAny<Stopwatch>()));
        }

        [Fact]
        public void Construct()
        {
            Assert.False(string.IsNullOrWhiteSpace(GlimpseRuntime.Version));
        }

        [Fact]
        public void ThrowsExceptionIfEndRequestIsCalledBeforeBeginRequest()
        {
            Runtime.Initialize();
            //runtime.BeginRequest(); commented out on purpose for this test

            Assert.Throws<GlimpseException>(() => Runtime.EndRequest());
        }

        [Fact]
        public void ThrowsExceptionIfBeginRequestIsCalledBeforeInittialize()
        {
            //Runtime.Initialize();commented out on purpose for this test
            
            Assert.Throws<GlimpseException>(() => Runtime.BeginRequest());
        }

        [Fact]
        public void ExecutePluginsWithDefaultLifeCycle()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();

            Runtime.BeginRequest();
            Runtime.EndRequest();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void ExecutePluginsWithLifeCycleMismatch()
        {
            Runtime.TabMock.Setup(m => m.ExecuteOn).Returns(RuntimeEvent.EndRequest);

            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact]
        public void ExecutePluginsMakeSureNamesAreJsonSafe()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);
            Assert.Contains("castle_proxies_itabproxy", results.First().Key); 
        }

        [Fact]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

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
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

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
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

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
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(1, results.Count);

            Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
            
            // Make sure the excption type above is logged here.
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact]
        public void ExecutePluginsWithEmptyCollection()
        {
            Runtime.Configuration.Tabs.Clear();
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            var results = Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.PluginResultsDataStoreKey);
            Assert.NotNull(results);
            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void FlagsTest()
        {
            //This test is just to help me keep my sanity with bitwise operators
            var support = RuntimeEvent.EndRequest;

            Assert.True(support.HasFlag(RuntimeEvent.EndRequest), "End is End");

            support = RuntimeEvent.EndRequest | RuntimeEvent.EndSessionAccess;

            Assert.True(support.HasFlag(RuntimeEvent.EndRequest), "End in End|SessionEnd");
            Assert.True(support.HasFlag(RuntimeEvent.EndSessionAccess), "SessionEnd in End|SessionEnd");
            //support End OR Begin
            Assert.True(support.HasFlag(RuntimeEvent.EndRequest & RuntimeEvent.BeginRequest), "End|Begin in End|SessionEnd");
            //support End AND SessionEnd
            Assert.True(support.HasFlag(RuntimeEvent.EndRequest | RuntimeEvent.EndSessionAccess), "End|SessionEnd in End|SessionEnd");
            Assert.False(support.HasFlag(RuntimeEvent.EndRequest | RuntimeEvent.BeginRequest), "End|Begin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(RuntimeEvent.BeginRequest), "Begin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(RuntimeEvent.BeginSessionAccess), "SessionBegin NOT in End|SessionEnd");
            Assert.False(support.HasFlag(RuntimeEvent.BeginRequest | RuntimeEvent.BeginSessionAccess), "Begin|SessionBegin NOT in End|SessionEnd");
        }

        [Fact]
        public void HaveASemanticVersion()
        {
            Version version;
            Assert.True(Version.TryParse(GlimpseRuntime.Version, out version));
            Assert.NotNull(version.Major);
            Assert.NotNull(version.Minor);
            Assert.NotNull(version.Build);
            Assert.Equal(-1, version.Revision);
        }

        [Fact]
        public void InitializeWithSetupTabs()
        {
            var setupMock = Runtime.TabMock.As<ITabSetup>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(It.IsAny<ITabSetupContext>()), Times.Once());
        }

        [Fact]
        public void InitializeWithSetupTabThatFails()
        {
            var setupMock = Runtime.TabMock.As<ITabSetup>();
            setupMock.Setup(s => s.Setup(new Mock<ITabSetupContext>().Object)).Throws<DummyException>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            Runtime.Initialize();

            setupMock.Verify(pm => pm.Setup(It.IsAny<ITabSetupContext>()), Times.Once());
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
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            Runtime.FrameworkProviderMock.Verify(fp => fp.InjectHttpResponseBody(It.IsAny<string>()));
        }
        
        [Fact]
        public void PersistDataDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            Runtime.PersistenceStoreMock.Verify(ps => ps.Save(It.IsAny<GlimpseRequest>()));
        }

        [Fact]
        public void SetResponseHeaderDuringEndRequest()
        {
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Initialize();
            Runtime.BeginRequest();
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

            Runtime.ExecuteResource(name.ToLower(), new ResourceParameters(new[]{"One","Two"}));

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

            Runtime.ExecuteResource(name.ToLower(), new ResourceParameters(new Dictionary<string, string>{{"One", "1"}, {"Two","2"}}));

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact]
        public void HandleUnknownResource()
        {
            Runtime.Configuration.Resources.Clear();

            Runtime.ExecuteResource("random name that doesn't exist", new ResourceParameters(new string[]{}));

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(404), Times.Once());
        }

        [Fact]
        public void HandleDuplicateResources()
        {
            var name = "Duplicate";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new ResourceParameters(new string[]{}));

            Runtime.FrameworkProviderMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact]
        public void ThrowExceptionWithEmptyResourceName()
        {
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource("", new ResourceParameters(new string[]{})));
        }

        [Fact]
        public void HandleResourcesThatThrowExceptions()
        {
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(name, new ResourceParameters(new string[]{}));

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

            Runtime.ExecuteResource(name, new ResourceParameters(new Dictionary<string, string>()));

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

            Runtime.ExecuteResource(name, new ResourceParameters(new string[]{}));

            Runtime.LoggerMock.Verify(l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()), Times.Once());
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
            var offPolicyMock = new Mock<IRuntimePolicy>();
            offPolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.Off);
            offPolicyMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.Initialize);

            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);
            Runtime.Configuration.RuntimePolicies.Add(offPolicyMock.Object);

            var result = Runtime.Initialize();

            Assert.False(result);
        }

        [Fact]
        public void NotIncreaseModeOverLifetimeOfRequest()
        {
            var glimpseMode = RuntimePolicy.ModifyResponseBody;
            Runtime.Configuration.DefaultRuntimePolicy = glimpseMode;

            var firstMode = Runtime.Initialize();

            Assert.True(firstMode);

            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.On;

            Runtime.BeginRequest();

            Assert.Equal(glimpseMode, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact]
        public void RespectConfigurationSettingInValidators()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

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
            Runtime.Initialize();
            Runtime.BeginRequest();

            Runtime.RuntimePolicyMock.Verify(v=>v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.AtLeastOnce());
        }

        [Fact]
        public void SkipEecutingInitializeIfGlimpseModeIfOff()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            
            Runtime.Initialize();
            
            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact] //False result means GlimpseMode == Off
        public void WriteCurrentModeToRequestState()
        {
            Runtime.RuntimePolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.ModifyResponseBody);
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            var result = Runtime.Initialize();

            Assert.True(result);

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact]
        public void SkipInitializeIfGlipseModeIsOff()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

            Runtime.Initialize();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact]
        public void SkipExecutingResourceIfGlipseModeIsOff()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

            Runtime.ExecuteResource("doesn't matter", new ResourceParameters(new string[]{}));

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact]
        public void ValidateAtEndRequest()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

            Runtime.EndRequest();

            Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.FrameworkProvider.HttpRequestStore.Get(Constants.RuntimePolicyKey));
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

/*        [Fact]
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
            
            Runtime.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagWithOneStaticResource()
        {
            var uri = "http://localhost/static";
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(GlimpseRuntime.Version)).Returns(uri);
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
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns("http://localhost/dynamic").Callback(()=>Assert.Equal(callCount++, 1));
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(GlimpseRuntime.Version)).Returns("http://localhost/static").Callback(()=>Assert.Equal(callCount++, 0));
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("http://localhost/static")).Returns("http://localhost/static/encoded");

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.NotEmpty(Runtime.GenerateScriptTags(Guid.NewGuid()));
        }

        [Fact]
        public void GenerateScriptTagsWithParameterValueProvider()
        {
            var resourceName = "resourceName";
            var uri = "http://somethingEncoded";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(resourceName);
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns(resourceName);
            var parameterValueProviderMock = Runtime.DynamicScriptMock.As<IParameterValueProvider>();
            Runtime.EndpointConfigMock.Protected().Setup<string>("GenerateUriTemplate", resourceName, "~/Glimpse.axd", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>()).Returns("http://something");
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("http://something")).Returns(uri);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Contains(uri, Runtime.GenerateScriptTags(Guid.NewGuid()));

            parameterValueProviderMock.Verify(vp=>vp.OverrideParameterValues(It.IsAny<IDictionary<string,string>>()));
        }

        [Fact]
        public void GenerateScriptTagsWithDynamicScriptAndMatchingResource()
        {
            var resourceName = "resourceName";
            var uri = "http://somethingEncoded";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(resourceName);
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns(resourceName);
            Runtime.EndpointConfigMock.Protected().Setup<string>("GenerateUriTemplate", resourceName, "~/Glimpse.axd", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>()).Returns("http://something");
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("http://something")).Returns(uri);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Contains(uri, Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.ResourceMock.Verify(rm=>rm.Name, Times.AtLeastOnce());
            Runtime.EndpointConfigMock.Protected().Verify<string>("GenerateUriTemplate", Times.Once(), resourceName, "~/Glimpse.axd", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>());
            Runtime.EncoderMock.Verify(e => e.HtmlAttributeEncode("http://something"), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenEndpointConfigReturnsEmptyString()
        {
            var resourceName = "resourceName";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(resourceName);
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns(resourceName);
            Runtime.EndpointConfigMock.Protected().Setup<string>("GenerateUriTemplate", resourceName, "~/Glimpse.axd", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>()).Returns("");
            Runtime.EncoderMock.Setup(e => e.HtmlAttributeEncode("")).Returns("");

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.ResourceMock.Verify(rm => rm.Name, Times.AtLeastOnce());
            Runtime.EndpointConfigMock.Protected().Verify<string>("GenerateUriTemplate", Times.Once(), resourceName, "~/Glimpse.axd", ItExpr.IsAny<IEnumerable<ResourceParameterMetadata>>(), ItExpr.IsAny<ILogger>());
            Runtime.EncoderMock.Verify(e => e.HtmlAttributeEncode(""), Times.Once());
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenMatchingResourceNotFound()
        {
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns("resourceName");

            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));

            Runtime.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void GenerateScriptTagsSkipsWhenStaticScriptReturnsEmptyString()
        {
            Runtime.StaticScriptMock.Setup(ss => ss.GetUri(GlimpseRuntime.Version)).Returns("");

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);

            Assert.Empty(Runtime.GenerateScriptTags(Guid.NewGuid()));
        }*/

        [Fact]
        public void LogErrorOnPersistenceStoreException()
        {
            Runtime.PersistenceStoreMock.Setup(ps => ps.Save(It.IsAny<GlimpseRequest>())).Throws<DummyException>();

            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void LogWarningWhenRuntimePolicyThrowsException()
        {
            Runtime.RuntimePolicyMock.Setup(pm => pm.Execute(It.IsAny<IRuntimePolicyContext>())).Throws<DummyException>();

            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            Assert.False(Runtime.Initialize());

            Runtime.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void LogErrorWhenDynamicScriptTagThrowsException()
        {
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Throws<DummyException>();

            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }


        [Fact]
        public void LogErrorWhenStaticScriptTagThrowsException()
        {
            Runtime.StaticScriptMock.Setup(ds => ds.GetUri(It.IsAny<string>())).Throws<DummyException>();

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);

            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndRequest();

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void BeginRuntimeReturnsEarlyIfRuntimePolicyIsOff()
        {
            Runtime.FrameworkProviderMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);

            Runtime.Initialize();

            Runtime.RuntimePolicyMock.Setup(p => p.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.Off);
            Runtime.RuntimePolicyMock.Setup(p => p.ExecuteOn).Returns(RuntimeEvent.BeginRequest);
            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            Runtime.BeginRequest();

            Runtime.HttpRequestStoreMock.Verify(fp=>fp.Set(Constants.RequestIdKey, It.IsAny<Guid>()), Times.Never());
            Runtime.HttpRequestStoreMock.Verify(fp=>fp.Set(Constants.GlobalStopwatchKey, It.IsAny<Stopwatch>()), Times.Never());
        }

        [Fact]
        public void ExecuteTabsOnBeginSessionAccess()
        {
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.BeginSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.BeginSessionAccess();

            Runtime.TabMock.Verify(t=>t.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void ExecuteTabsOnEndSessionAccess()
        {
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.EndSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.Initialize();
            Runtime.BeginRequest();
            Runtime.EndSessionAccess();

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact]
        public void StopBeginSessionAccessWithRuntimePolicyOff()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.BeginSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginSessionAccess();

            Runtime.TabMock.Verify(t=>t.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact]
        public void StopEndSessionAccessWithRuntimePolicyOff()
        {
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.EndSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.EndSessionAccess();

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact]
        public void ThrowExceptionWhenExecutingResourceWithNullParameters()
        {
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource("any", null));
        }
    }
}