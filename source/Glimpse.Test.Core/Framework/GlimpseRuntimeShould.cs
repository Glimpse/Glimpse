using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Test.Common;
using Glimpse.Test.Core.TestDoubles;
using Glimpse.Test.Core.Tester;
using Moq;
using Xunit;
using Xunit.Extensions;
using Glimpse.Test.Core.Extensions;

namespace Glimpse.Test.Core.Framework
{
    public class GlimpseRuntimeShould : IDisposable
    {
        public GlimpseRuntimeShould()
        {
            Runtime = GlimpseRuntimeTester.Create();
        }

        private GlimpseRuntimeTester Runtime { get; set; }

        public void Dispose()
        {
            Runtime = null;
            GlimpseRuntime.Reset();
        }

        private GlimpseRequestContextHandle CreateGlimpseRequestContextHandle(RequestHandlingMode requestHandlingMode = RequestHandlingMode.RegularRequest)
        {
            return requestHandlingMode == RequestHandlingMode.Unhandled
                    ? UnavailableGlimpseRequestContextHandle.Instance
                    : new GlimpseRequestContextHandle(Guid.NewGuid(), requestHandlingMode, () => { });
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void SetRequestIdOnBeginRequest()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //providerMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);

            //Runtime.BeginRequest(providerMock.Object);

            //Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.RequestIdKey, It.IsAny<Guid>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void StartGlobalStopwatchOnBeginRequest()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //providerMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);
            //Runtime.BeginRequest(providerMock.Object);

            //Runtime.HttpRequestStoreMock.Verify(store => store.Set(Constants.GlobalStopwatchKey, It.IsAny<Stopwatch>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void Construct()
        {
            Assert.False(string.IsNullOrWhiteSpace(GlimpseRuntime.Version));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ThrowsExceptionIfEndRequestIsCalledBeforeBeginRequest()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            //runtime.BeginRequest(); commented out on purpose for this test

            Assert.Throws<GlimpseException>(() => Runtime.EndRequest(CreateGlimpseRequestContextHandle()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ThrowsExceptionIfBeginRequestIsCalledBeforeInitialize()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            //Runtime.Initialize();commented out on purpose for this test

            Assert.Throws<GlimpseException>(() => Runtime.BeginRequest(providerMock.Object));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithDefaultLifeCycle()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithLifeCycleMismatch()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            //Runtime.TabMock.Setup(m => m.ExecuteOn).Returns(RuntimeEvent.EndRequest);

            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(0, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsMakeSureNamesAreJsonSafe()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);
            //Assert.Contains("castle_proxies_itabproxy", results.First().Key);
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithMatchingRuntimeContextType()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithUnknownRuntimeContextType()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.TabMock.Setup(m => m.RequestContextType).Returns<Type>(null);

            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithDuplicateCollectionEntries()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            ////Insert the same plugin multiple times
            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.AtLeastOnce());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginThatFails()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.TabMock.Setup(p => p.GetData(It.IsAny<ITabContext>())).Throws<DummyException>();

            //Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(1, results.Count);

            //Runtime.TabMock.Verify(p => p.GetData(It.IsAny<ITabContext>()), Times.Once());

            //// Make sure the exception type above is logged here.
            //Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecutePluginsWithEmptyCollection()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.Configuration.Tabs.Clear();
            //Runtime.BeginRequest(providerMock.Object);
            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //var results = providerMock.Object.HttpRequestStore.Get<IDictionary<string, TabResult>>(Constants.TabResultsDataStoreKey);
            //Assert.NotNull(results);
            //Assert.Equal(0, results.Count);
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
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

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void HaveASemanticVersion()
        {
            Version version;
            Assert.True(Version.TryParse(GlimpseRuntime.Version, out version));
            Assert.NotNull(version.Major);
            Assert.NotNull(version.Minor);
            Assert.NotNull(version.Build);
            Assert.Equal(-1, version.Revision);
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InitializeWithSetupTabs()
        {
            var setupMock = Runtime.TabMock.As<ITabSetup>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            setupMock.Verify(pm => pm.Setup(It.IsAny<ITabSetupContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InitializeWithSetupTabThatFails()
        {
            var setupMock = Runtime.TabMock.As<ITabSetup>();
            setupMock.Setup(s => s.Setup(new Mock<ITabSetupContext>().Object)).Throws<DummyException>();

            //one tab needs setup, the other does not
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.Configuration.Tabs.Add(new DummyTab());

            setupMock.Verify(pm => pm.Setup(It.IsAny<ITabSetupContext>()), Times.Once());
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Tabs.Count));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InitializeWithInspectors()
        {
            Runtime.Configuration.Inspectors.Add(Runtime.InspectorMock.Object);

            Runtime.InspectorMock.Verify(pi => pi.Setup(It.IsAny<IInspectorContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InitializeWithInspectorThatFails()
        {
            Runtime.InspectorMock.Setup(pi => pi.Setup(It.IsAny<IInspectorContext>())).Throws<DummyException>();

            Runtime.Configuration.Inspectors.Add(Runtime.InspectorMock.Object);

            Runtime.InspectorMock.Verify(pi => pi.Setup(It.IsAny<IInspectorContext>()), Times.Once());
            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>()), Times.AtMost(Runtime.Configuration.Inspectors.Count));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InjectHttpResponseBodyDuringEndRequest()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();

            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            providerMock.Verify(fp => fp.InjectHttpResponseBody(It.IsAny<string>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void PersistDataDuringEndRequest()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            Runtime.PersistenceStoreMock.Verify(ps => ps.Save(It.IsAny<GlimpseRequest>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void SetResponseHeaderDuringEndRequest()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);
            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            providerMock.Verify(fp => fp.SetHttpResponseHeader(Constants.HttpResponseHeader, It.IsAny<string>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecuteResourceWithOrderedParameters()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name.ToLower(), new ResourceParameters(new[] { "One", "Two" }));

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecuteResourceWithNamedParameters()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "TestResource";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name.ToLower(), new ResourceParameters(new Dictionary<string, string> { { "One", "1" }, { "Two", "2" } }));

            Runtime.ResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), Times.Once());
            Runtime.ResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void HandleUnknownResource()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.Configuration.Resources.Clear();

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), "random name that doesn't exist", new ResourceParameters(new string[] { }));

            providerMock.Verify(fp => fp.SetHttpResponseStatusCode(404), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void HandleDuplicateResources()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "Duplicate";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);
            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name, new ResourceParameters(new string[] { }));

            providerMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ThrowExceptionWithEmptyResourceName()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), "", new ResourceParameters(new string[] { })));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void HandleResourcesThatThrowExceptions()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name, new ResourceParameters(new string[] { }));

            providerMock.Verify(fp => fp.SetHttpResponseStatusCode(500), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void EnsureNullIsNotPassedToResourceExecute()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "aName";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(
                Runtime.ResourceResultMock.Object);

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name, new ResourceParameters(new Dictionary<string, string>()));

            Runtime.ResourceMock.Verify(r => r.Execute(null), Times.Never());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void HandleResourceResultsThatThrowExceptions()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            var name = "Anything";
            Runtime.ResourceMock.Setup(r => r.Name).Returns(name);
            Runtime.ResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(Runtime.ResourceResultMock.Object);

            Runtime.ResourceResultMock.Setup(rr => rr.Execute(It.IsAny<IResourceResultContext>())).Throws<Exception>();

            Runtime.Configuration.Resources.Add(Runtime.ResourceMock.Object);

            Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), name, new ResourceParameters(new string[] { }));

            Runtime.LoggerMock.Verify(l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()), Times.Once());
        }

        /*
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
        */

        /*
                [Fact]
                public void NotIncreaseModeOverLifetimeOfRequest()
                {
                    var providerMock = new Mock<IRequestResponseAdapter>().Setup();
                    var glimpseMode = RuntimePolicy.ModifyResponseBody;
                    Runtime.Configuration.DefaultRuntimePolicy = glimpseMode;

                    var firstMode = Runtime.Initialize();

                    Assert.True(firstMode);

                    Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.On;

                    Runtime.BeginRequest(providerMock.Object);

                    Assert.Equal(glimpseMode, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */


        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ValidateAtBeginRequest()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.RuntimePolicyMock.Setup(rp => rp.ExecuteOn).Returns(RuntimeEvent.BeginRequest);

            Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);
            Runtime.BeginRequest(providerMock.Object);

            Runtime.RuntimePolicyMock.Verify(v => v.Execute(It.IsAny<IRuntimePolicyContext>()), Times.AtLeastOnce());
        }

        /*
                [Fact]
                public void SkipEecutingInitializeIfGlimpseModeIfOff()
                {
                    var providerMock = new Mock<IRequestResponseAdapter>().Setup();
                    Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            
                    Runtime.Initialize();
            
                    Assert.Equal(RuntimePolicy.Off, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */

        /*
                [Fact] //False result means GlimpseMode == Off
                public void WriteCurrentModeToRequestState()
                {
                    var providerMock = new Mock<IRequestResponseAdapter>().Setup();
                    Runtime.RuntimePolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.ModifyResponseBody);
                    Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

                    var result = Runtime.Initialize();

                    Assert.True(result);

                    Assert.Equal(RuntimePolicy.ModifyResponseBody, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */

        /*
                [Fact]
                public void SkipInitializeIfGlipseModeIsOff()
                {
                    var providerMock = new Mock<IRequestResponseAdapter>().Setup();
                    Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

                    Runtime.Initialize();

                    Assert.Equal(RuntimePolicy.Off, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void SkipExecutingResourceIfGlimpseModeIsOff()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

            //Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), "doesn't matter", new ResourceParameters(new string[] { }));

            //Assert.Equal(RuntimePolicy.Off, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ValidateAtEndRequest()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;

            //Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            //Assert.Equal(RuntimePolicy.Off, providerMock.Object.HttpRequestStore.Get(Constants.RuntimePolicyKey));
        }

        /*
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
        */

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

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void LogErrorOnPersistenceStoreException()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.PersistenceStoreMock.Setup(ps => ps.Save(It.IsAny<GlimpseRequest>())).Throws<DummyException>();

            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        /*
                [Fact]
                public void LogWarningWhenRuntimePolicyThrowsException()
                {
                    Runtime.RuntimePolicyMock.Setup(pm => pm.Execute(It.IsAny<IRuntimePolicyContext>())).Throws<DummyException>();

                    Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

                    Assert.False(Runtime.Initialize());

                    Runtime.LoggerMock.Verify(l => l.Warn(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
                }
        */

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void LogErrorWhenDynamicScriptTagThrowsException()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.DynamicScriptMock.Setup(ds => ds.GetResourceName()).Throws<DummyException>();

            Runtime.Configuration.ClientScripts.Add(Runtime.DynamicScriptMock.Object);

            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }


        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void LogErrorWhenStaticScriptTagThrowsException()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.StaticScriptMock.Setup(ds => ds.GetUri(It.IsAny<string>())).Throws<DummyException>();

            Runtime.Configuration.ClientScripts.Add(Runtime.StaticScriptMock.Object);

            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndRequest(CreateGlimpseRequestContextHandle());

            Runtime.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<DummyException>(), It.IsAny<object[]>()));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void BeginRuntimeReturnsEarlyIfRuntimePolicyIsOff()
        {
#warning test might become obsolete due to complete redesign of GlimpseRuntime
            //var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            //providerMock.Setup(fp => fp.HttpRequestStore).Returns(Runtime.HttpRequestStoreMock.Object);


            //Runtime.RuntimePolicyMock.Setup(p => p.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.Off);
            //Runtime.RuntimePolicyMock.Setup(p => p.ExecuteOn).Returns(RuntimeEvent.BeginRequest);
            //Runtime.Configuration.RuntimePolicies.Add(Runtime.RuntimePolicyMock.Object);

            //Runtime.BeginRequest(providerMock.Object);

            //Runtime.HttpRequestStoreMock.Verify(fp => fp.Set(Constants.RequestIdKey, It.IsAny<Guid>()), Times.Never());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecuteTabsOnBeginSessionAccess()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.BeginSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest(providerMock.Object);
            Runtime.BeginSessionAccess(CreateGlimpseRequestContextHandle());

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ExecuteTabsOnEndSessionAccess()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.EndSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginRequest(providerMock.Object);
            Runtime.EndSessionAccess(CreateGlimpseRequestContextHandle());

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Once());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void StopBeginSessionAccessWithRuntimePolicyOff()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.BeginSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.BeginSessionAccess(CreateGlimpseRequestContextHandle());

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void StopEndSessionAccessWithRuntimePolicyOff()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.Off;
            Runtime.TabMock.Setup(t => t.ExecuteOn).Returns(RuntimeEvent.EndSessionAccess);
            Runtime.Configuration.Tabs.Add(Runtime.TabMock.Object);

            Runtime.EndSessionAccess(CreateGlimpseRequestContextHandle());

            Runtime.TabMock.Verify(t => t.GetData(It.IsAny<ITabContext>()), Times.Never());
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ThrowExceptionWhenExecutingResourceWithNullParameters()
        {
            var providerMock = new Mock<IRequestResponseAdapter>().Setup();
            Assert.Throws<ArgumentNullException>(() => Runtime.ExecuteResource(CreateGlimpseRequestContextHandle(), "any", null));
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void ThrowExceptionWhenAccessingNonInitializedInstance()
        {
            Assert.Throws<GlimpseException>(() => GlimpseRuntime.Instance);
        }

        [Theory(Skip = "This test is hanging the test runner. Fix later"), AutoMock]
        public void InitializeSetsInstanceWhenExecuted(IGlimpseConfiguration configuration)
        {
            GlimpseRuntime.Initialize(configuration);

            Assert.NotNull(GlimpseRuntime.Instance);
        }

        [Theory(Skip = "This test is hanging the test runner. Fix later"), AutoMock]
        public void InitializeSetsConfigurationWhenExecuted(IGlimpseConfiguration configuration)
        {
            GlimpseRuntime.Initialize(configuration);

            Assert.Equal(configuration, GlimpseRuntime.Instance.Configuration);
        }

        [Fact(Skip = "This test is hanging the test runner. Fix later")]
        public void InitializeThrowsWithNullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() => GlimpseRuntime.Initialize(null));
        }

        /*
         * The following tests are tests related to they way runtime policies are evaluated in case resources are being executed, but they also
         * cover the way normal runtime policies will be evaluated. Below you'll find a table that describes the test cases below
         * 
         * -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         * DefaultRuntimePolicy | BeginRequestPolicy1 | BeginRequestPolicy2	| ResourcePolicy1                                         | ResourcePolicy2                                         | Default Resource                 | Other Resource
         * -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         *                      | Returns | Executed? | Returns | Executed? | Returns             | Executed?                         | Returns             | Executed?                         | RuntimePolicy Result | Executed? | RuntimePolicy Result | Executed?
         * -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
         * Off                  |         | false     |         | false     |                     | false                             |                     | false                             | Off                  | false     | Off                  | false
         * On                   | On      | true      | On      | true      | ExecuteResourceOnly | true                              | ExecuteResourceOnly | true                              | ExecuteResourceOnly  | true      | ExecuteResourceOnly  | true
         * On                   | On      | true      | On      | true      | ExecuteResourceOnly | true                              | Off                 | true                              | Off                  | false     | Off                  | false
         * On                   | On      | true      | On      | true      | Off                 | true                              |                     | false                             | Off                  | false     | Off                  | false
         * On                   | On      | true      | Off     | true      | ExecuteResourceOnly | true (only when default resource) | ExecuteResourceOnly | true (only when default resource) | Off                  | true      | Off                  | false
         * On                   | Off     | true      |         | false     | ExecuteResourceOnly | true (only when default resource) | ExecuteResourceOnly | true (only when default resource) | Off                  | true      | Off                  | false
         */

        /*
        [Fact]
        public void SkipExecutionOfDefaultResourceWhenDefaultRuntimePolicyIsOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true,
                    DefaultRuntimePolicy = RuntimePolicy.Off,
                    FirstRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    SecondRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    FirstRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void SkipExecutionOfNonDefaultResourcesWhenDefaultRuntimePolicyIsOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = false,
                    DefaultRuntimePolicy = RuntimePolicy.Off,
                    FirstRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    SecondRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    FirstRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void ExecuteDefaultResourceWhenDefaultRuntimePolicyIsOnAndNoOtherRuntimePolicySaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true
                });
        }

        [Fact]
        public void ExecuteNonDefaultResourcesWhenDefaultRuntimePolicyIsOnAndNoOtherRuntimePolicySaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase());
        }

        [Fact]
        public void SkipExecutingDefaultResourceWhenFirstRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true,
                    RuntimePolicyReturnedByFirstRuntimePolicyOnExecuteResource = RuntimePolicy.Off,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void SkipExecutingDefaultResourceWhenSecondRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true,
                    RuntimePolicyReturnedBySecondRuntimePolicyOnExecuteResource = RuntimePolicy.Off,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void SkipExecutingNonDefaultResourcesWhenFirstRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    RuntimePolicyReturnedByFirstRuntimePolicyOnExecuteResource = RuntimePolicy.Off,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void SkipExecutingNonDefaultResourcesWhenSecondRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    RuntimePolicyReturnedBySecondRuntimePolicyOnExecuteResource = RuntimePolicy.Off,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void ExecuteDefaultResourceEvenWhenSecondRuntimePolicyOnBeginRequestSaidOffAndNoRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true,
                    RuntimePolicyReturnedBySecondRuntimePolicyOnBeginRequest = RuntimePolicy.Off,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off
                });
        }

        [Fact]
        public void SkipExecutingNonDefaultResourcesWhenSecondRuntimePolicyOnBeginRequestSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    RuntimePolicyReturnedBySecondRuntimePolicyOnBeginRequest = RuntimePolicy.Off,
                    FirstRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }

        [Fact]
        public void ExecuteDefaultResourceEvenWhenFirstRuntimePolicyOnBeginRequestSaidOffAndNoRuntimePolicyOnExecuteResourceSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    CheckDefaultResourceAccess = true,
                    RuntimePolicyReturnedByFirstRuntimePolicyOnBeginRequest = RuntimePolicy.Off,
                    SecondRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    FirstRuntimePolicyOnExecuteResourceMustBeExecuted = true,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = true,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off
                });
        }

        [Fact]
        public void SkipExecutingNonDefaultResourcesWhenFirstRuntimePolicyOnBeginRequestSaidOff()
        {
            ValidateResourceExecutionAndResultingRuntimePolicy(
                new ResourceExecutionAndResultingRuntimePolicyTestCase
                {
                    RuntimePolicyReturnedByFirstRuntimePolicyOnBeginRequest = RuntimePolicy.Off,
                    SecondRuntimePolicyOnBeginRequestMustBeExecuted = false,
                    FirstRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    SecondRuntimePolicyOnExecuteResourceMustBeExecuted = false,
                    ResultingRuntimePolicyForResource = RuntimePolicy.Off,
                    ResourceMustBeExecuted = false
                });
        }
         */

        private class ResourceExecutionAndResultingRuntimePolicyTestCase
        {
            public ResourceExecutionAndResultingRuntimePolicyTestCase()
            {
                DefaultRuntimePolicy = RuntimePolicy.On;
                RuntimePolicyReturnedByFirstRuntimePolicyOnBeginRequest = RuntimePolicy.On;
                FirstRuntimePolicyOnBeginRequestMustBeExecuted = true;
                RuntimePolicyReturnedBySecondRuntimePolicyOnBeginRequest = RuntimePolicy.On;
                SecondRuntimePolicyOnBeginRequestMustBeExecuted = true;
                RuntimePolicyReturnedByFirstRuntimePolicyOnExecuteResource = RuntimePolicy.ExecuteResourceOnly;
                FirstRuntimePolicyOnExecuteResourceMustBeExecuted = true;
                RuntimePolicyReturnedBySecondRuntimePolicyOnExecuteResource = RuntimePolicy.ExecuteResourceOnly;
                SecondRuntimePolicyOnExecuteResourceMustBeExecuted = true;

                CheckDefaultResourceAccess = false;
                ResultingRuntimePolicyForResource = RuntimePolicy.ExecuteResourceOnly;
                ResourceMustBeExecuted = true;
            }

            public RuntimePolicy DefaultRuntimePolicy { get; set; }
            public RuntimePolicy RuntimePolicyReturnedByFirstRuntimePolicyOnBeginRequest { get; set; }
            public bool FirstRuntimePolicyOnBeginRequestMustBeExecuted { get; set; }
            public RuntimePolicy RuntimePolicyReturnedBySecondRuntimePolicyOnBeginRequest { get; set; }
            public bool SecondRuntimePolicyOnBeginRequestMustBeExecuted { get; set; }

            public RuntimePolicy RuntimePolicyReturnedByFirstRuntimePolicyOnExecuteResource { get; set; }
            public bool FirstRuntimePolicyOnExecuteResourceMustBeExecuted { get; set; }
            public RuntimePolicy RuntimePolicyReturnedBySecondRuntimePolicyOnExecuteResource { get; set; }
            public bool SecondRuntimePolicyOnExecuteResourceMustBeExecuted { get; set; }

            public bool CheckDefaultResourceAccess { get; set; }
            public RuntimePolicy ResultingRuntimePolicyForResource { get; set; }
            public bool ResourceMustBeExecuted { get; set; }
        }

        /*
                private void ValidateResourceExecutionAndResultingRuntimePolicy(ResourceExecutionAndResultingRuntimePolicyTestCase testCase)
                {
                    var firstRuntimePolicyOnBeginRequestMock = new Mock<IRuntimePolicy>();
                    firstRuntimePolicyOnBeginRequestMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(testCase.RuntimePolicyReturnedByFirstRuntimePolicyOnBeginRequest);
                    firstRuntimePolicyOnBeginRequestMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.BeginRequest);
                    Runtime.Configuration.RuntimePolicies.Add(firstRuntimePolicyOnBeginRequestMock.Object);

                    var secondRuntimePolicyOnBeginRequestMock = new Mock<IRuntimePolicy>();
                    secondRuntimePolicyOnBeginRequestMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(testCase.RuntimePolicyReturnedBySecondRuntimePolicyOnBeginRequest);
                    secondRuntimePolicyOnBeginRequestMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.BeginRequest);
                    Runtime.Configuration.RuntimePolicies.Add(secondRuntimePolicyOnBeginRequestMock.Object);

                    var firstRuntimePolicyOnExecuteResourceMock = new Mock<IRuntimePolicy>();
                    firstRuntimePolicyOnExecuteResourceMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(testCase.RuntimePolicyReturnedByFirstRuntimePolicyOnExecuteResource);
                    firstRuntimePolicyOnExecuteResourceMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.ExecuteResource);
                    Runtime.Configuration.RuntimePolicies.Add(firstRuntimePolicyOnExecuteResourceMock.Object);

                    var secondRuntimePolicyOnExecuteResourceMock = new Mock<IRuntimePolicy>();
                    secondRuntimePolicyOnExecuteResourceMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(testCase.RuntimePolicyReturnedBySecondRuntimePolicyOnExecuteResource);
                    secondRuntimePolicyOnExecuteResourceMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.ExecuteResource);
                    Runtime.Configuration.RuntimePolicies.Add(secondRuntimePolicyOnExecuteResourceMock.Object);

                    Runtime.Configuration.DefaultRuntimePolicy = testCase.DefaultRuntimePolicy;

                    var defaultResourceMock = new Mock<IResource>();
                    defaultResourceMock.Setup(r => r.Name).Returns("defaultResourceName");
                    var defaultResourceResultMock = new Mock<IResourceResult>();
                    defaultResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(defaultResourceResultMock.Object);
                    Runtime.Configuration.Resources.Add(defaultResourceMock.Object);
                    Runtime.Configuration.DefaultResource = defaultResourceMock.Object;

                    var normalResourceMock = new Mock<IResource>();
                    normalResourceMock.Setup(r => r.Name).Returns("normalResourceName");
                    var normalResourceResultMock = new Mock<IResourceResult>();
                    normalResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(normalResourceResultMock.Object);
                    Runtime.Configuration.Resources.Add(normalResourceMock.Object);

                    Runtime.Initialize();
                    Runtime.BeginRequest();

                    if (testCase.CheckDefaultResourceAccess)
                    {
                        Runtime.ExecuteDefaultResource();
                        defaultResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), testCase.ResourceMustBeExecuted ? Times.Once() : Times.Never());
                        defaultResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()), testCase.ResourceMustBeExecuted ? Times.Once() : Times.Never());
                    }
                    else
                    {
                        Runtime.ExecuteResource("normalResourceName", new ResourceParameters(new Dictionary<string, string>()));
                        normalResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), testCase.ResourceMustBeExecuted ? Times.Once() : Times.Never());
                        normalResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()), testCase.ResourceMustBeExecuted ? Times.Once() : Times.Never());
                    }

                    firstRuntimePolicyOnBeginRequestMock.Verify(v => v.Execute(It.IsAny<IRuntimePolicyContext>()), testCase.FirstRuntimePolicyOnBeginRequestMustBeExecuted ? Times.AtLeastOnce() : Times.Never());
                    secondRuntimePolicyOnBeginRequestMock.Verify(v => v.Execute(It.IsAny<IRuntimePolicyContext>()), testCase.SecondRuntimePolicyOnBeginRequestMustBeExecuted ? Times.AtLeastOnce() : Times.Never());
                    firstRuntimePolicyOnExecuteResourceMock.Verify(v => v.Execute(It.IsAny<IRuntimePolicyContext>()), testCase.FirstRuntimePolicyOnExecuteResourceMustBeExecuted ? Times.AtLeastOnce() : Times.Never());
                    secondRuntimePolicyOnExecuteResourceMock.Verify(v => v.Execute(It.IsAny<IRuntimePolicyContext>()), testCase.SecondRuntimePolicyOnExecuteResourceMustBeExecuted ? Times.AtLeastOnce() : Times.Never());

                    Assert.Equal(testCase.ResultingRuntimePolicyForResource, Runtime.Configuration.requestResponseAdapter.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */

        private class MyResourceWithDependencies : IResource, IDependOnResources
        {
            private string DependsOnResourceName { get; set; }

            public MyResourceWithDependencies(string dependsOnResourceName)
            {
                DependsOnResourceName = dependsOnResourceName;
            }

            public bool DependsOn(string resourceName)
            {
                return resourceName == DependsOnResourceName;
            }

            public string Name
            {
                get { return "MyResourceWithDependencies"; }
            }

            public IEnumerable<ResourceParameterMetadata> Parameters
            {
                get { return new ResourceParameterMetadata[0]; }
            }

            public IResourceResult Execute(IResourceContext context)
            {
                return null; // should not be executed during tests
            }
        }

        /*
                [Fact]
                public void ExecuteResourceIfItIsADependencyOfTheDefaultResource()
                {
                    ExecuteResourceDependencyTest("dependentResourceName", "dependentResourceName", true);
                }

                [Fact]
                public void SkipExecutionOfResourceIfItIsNotDependencyOfTheDefaultResource()
                {
                    ExecuteResourceDependencyTest("someOtherDependentResourceName", "dependentResourceName", false);
                }

                private void ExecuteResourceDependencyTest(string resourceToExecute, string dependentResourceName, bool shouldHaveExecuted)
                {
                    Runtime.Configuration.DefaultRuntimePolicy = RuntimePolicy.On;
                    Runtime.Configuration.requestResponseAdapter.HttpRequestStore.Set(Constants.RuntimePolicyKey, RuntimePolicy.Off);

                    var defaultResource = new MyResourceWithDependencies(dependentResourceName);
                    Runtime.Configuration.DefaultResource = defaultResource;
                    Runtime.Configuration.Resources.Add(defaultResource);

                    var dependentResourceMock = new Mock<IResource>();
                    dependentResourceMock.Setup(r => r.Name).Returns(dependentResourceName);
                    var dependentResourceResultMock = new Mock<IResourceResult>();
                    dependentResourceMock.Setup(r => r.Execute(It.IsAny<IResourceContext>())).Returns(dependentResourceResultMock.Object);
                    Runtime.Configuration.Resources.Add(dependentResourceMock.Object);

                    Runtime.ExecuteResource(resourceToExecute, new ResourceParameters(new Dictionary<string, string>()));
                    dependentResourceMock.Verify(r => r.Execute(It.IsAny<IResourceContext>()), shouldHaveExecuted ? Times.Once() : Times.Never());
                    dependentResourceResultMock.Verify(r => r.Execute(It.IsAny<IResourceResultContext>()), shouldHaveExecuted ? Times.Once() : Times.Never());

                    Assert.Equal(RuntimePolicy.Off, Runtime.Configuration.requestResponseAdapter.HttpRequestStore.Get(Constants.RuntimePolicyKey));
                }
        */

        /* End of tests related to they way runtime policies are evaluated in case resources are being executed */
    }
}