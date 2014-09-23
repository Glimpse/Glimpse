using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.Extensions;
using Moq;
using System;

namespace Glimpse.Test.Core.Tester
{
    public class GlimpseRuntimeTester
    {
        public Mock<ResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        public Mock<IDataStore> HttpRequestStoreMock { get; set; }
        public Mock<ITab> TabMock { get; set; }
        public Mock<IInspector> InspectorMock { get; set; }
        public Mock<ISerializer> SerializerMock { get; set; }
        public Mock<IPersistenceStore> PersistenceStoreMock { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }
        public Mock<IResource> ResourceMock { get; set; }
        public Mock<IResourceResult> ResourceResultMock { get; set; }
        public Mock<IRuntimePolicy> RuntimePolicyMock { get; set; }
        public Mock<IStaticClientScript> StaticScriptMock { get; set; }
        public Mock<IDynamicClientScript> DynamicScriptMock { get; set; }
        public Mock<IHtmlEncoder> EncoderMock { get; set; }

        public GlimpseRuntime GlimpseRuntime { get; private set; }

        private GlimpseRuntimeTester()
        {
            var loggerMock = new Mock<ILogger>();
            var serializerMock = new Mock<ISerializer>();
            var persistenceStoreMock = new Mock<IPersistenceStore>();
            var encoderMock = new Mock<IHtmlEncoder>();

            var resources = new ReflectionDiscoverableCollection<IResource>(loggerMock.Object);
            var tabs = new ReflectionDiscoverableCollection<ITab>(loggerMock.Object);
            var policies = new ReflectionDiscoverableCollection<IRuntimePolicy>(loggerMock.Object);

            var configuration = new Glimpse.Core.Framework.Configuration(new Mock<ResourceEndpointConfiguration>().Object,
                new Mock<IPersistenceStore>().Object)
            {
                Logger = loggerMock.Object,
                Resources = resources,
                Tabs = tabs,
                RuntimePolicies = policies,
                Serializer = serializerMock.Object,
                PersistenceStore = persistenceStoreMock.Object,
                DefaultRuntimePolicy = RuntimePolicy.On,
                HtmlEncoder = encoderMock.Object
            };

            var readonlyConfiguration = new ReadOnlyConfigurationAdapter(configuration);

            var activeGlimpseRequestContexts = new ActiveGlimpseRequestContexts(readonlyConfiguration.CurrentGlimpseRequestIdTracker);

            var displayProvider = new DisplayProvider(readonlyConfiguration, activeGlimpseRequestContexts);
            displayProvider.Setup();

            var tabProvider = new TabProvider(readonlyConfiguration, activeGlimpseRequestContexts);
            tabProvider.Setup();

            var inspectorProvider = new InspectorProvider(readonlyConfiguration, activeGlimpseRequestContexts);
            inspectorProvider.Setup();

            var metadataProvider = new MetadataProvider(readonlyConfiguration);
            metadataProvider.SaveMetadata();

            var runtimePolicyDeterminator = new RuntimePolicyDeterminator(readonlyConfiguration);

            GlimpseRuntime = new GlimpseRuntime(
                readonlyConfiguration,
                activeGlimpseRequestContexts,
                runtimePolicyDeterminator,
                metadataProvider,
                tabProvider,
                displayProvider);

            HttpRequestStoreMock = new Mock<IDataStore>();
            TabMock = new Mock<ITab>().Setup();
            InspectorMock = new Mock<IInspector>();
            SerializerMock = serializerMock;
            PersistenceStoreMock = persistenceStoreMock;
            LoggerMock = loggerMock;
            ResourceMock = new Mock<IResource>();
            ResourceResultMock = new Mock<IResourceResult>();
            RuntimePolicyMock = new Mock<IRuntimePolicy>();
            EncoderMock = encoderMock;

            RuntimePolicyMock.Setup(v => v.Execute(It.IsAny<IRuntimePolicyContext>())).Returns(RuntimePolicy.On);
            RuntimePolicyMock.Setup(v => v.ExecuteOn).Returns(RuntimeEvent.Initialize);

            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.RequestHttpMethod).Returns("GET");
            RequestMetadataMock.Setup(r => r.RequestIsAjax).Returns(true);
            RequestMetadataMock.Setup(r => r.RequestUri).Returns(new Uri("http://localhost"));
            RequestMetadataMock.Setup(r => r.ResponseStatusCode).Returns(200);
            RequestMetadataMock.Setup(r => r.ResponseContentType).Returns(@"text\html");
            RequestMetadataMock.Setup(r => r.GetCookie(Constants.ClientIdCookieName)).Returns(@"Some Client");
            RequestMetadataMock.Setup(r => r.GetCookie(Constants.UserAgentHeaderName)).Returns(@"FireFox");
            RequestMetadataMock.Setup(r => r.GetHttpHeader(Constants.HttpRequestHeader)).Returns(Guid.NewGuid().ToString());

            StaticScriptMock = new Mock<IStaticClientScript>();
            StaticScriptMock.Setup(ss => ss.Order).Returns(ScriptOrder.ClientInterfaceScript);

            DynamicScriptMock = new Mock<IDynamicClientScript>();
            DynamicScriptMock.Setup(ds => ds.Order).Returns(ScriptOrder.RequestDataScript);
            DynamicScriptMock.Setup(ds => ds.GetResourceName()).Returns("aResource");
        }

        public static GlimpseRuntimeTester Create()
        {
            return new GlimpseRuntimeTester();
        }
    }
}