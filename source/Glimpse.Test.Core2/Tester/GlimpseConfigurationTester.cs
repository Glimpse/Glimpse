using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Extensions;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class GlimpseConfigurationTester : GlimpseConfiguration
    {
        private GlimpseConfigurationTester(Mock<IFrameworkProvider> frameworkProviderMock,
                                           Mock<ResourceEndpointConfiguration> endpointConfigurationMock,
                                           IDiscoverableCollection<IClientScript> clientScriptsStub,
                                           Mock<ILogger> loggerMock,
                                           Mock<IHtmlEncoder> htmlEncoderMock,
                                           Mock<IPersistanceStore> persistanceStoreMock,
                                           IDiscoverableCollection<IPipelineInspector> pipelineInspectorsStub,
                                           IDiscoverableCollection<IResource> resourcesStub,
                                           Mock<ISerializer> serializerMock,
                                           IDiscoverableCollection<ITab> tabsStub,
                                           IDiscoverableCollection<IRuntimePolicy> policiesStub,
                                           Mock<IResource> defaultResourceMock,
                                           Mock<IProxyFactory> proxyFactoryMock,
            Mock<IMessageBroker> messageBrokerMock)
            : base(
                frameworkProviderMock.Object,
                endpointConfigurationMock.Object,
                clientScriptsStub,
                loggerMock.Object,
                RuntimePolicy.On,
                htmlEncoderMock.Object,
                persistanceStoreMock.Object,
                pipelineInspectorsStub,
                resourcesStub,
                serializerMock.Object,
                tabsStub,
                policiesStub,
                defaultResourceMock.Object,
                proxyFactoryMock.Object,
            messageBrokerMock.Object)
        {
            FrameworkProviderMock = frameworkProviderMock;
            EndpointConfigMock = endpointConfigurationMock;
            ClientScriptsStub = clientScriptsStub;
            LoggerMock = loggerMock;
            HtmlEncoderMock = htmlEncoderMock;
            PersistanceStoreMock = persistanceStoreMock;
            SerializerMock = serializerMock;
        }

        public static GlimpseConfigurationTester Create()
        {
            var loggerMock = new Mock<ILogger>();

            return new GlimpseConfigurationTester(new Mock<IFrameworkProvider>().Setup(),
                                                  new Mock<ResourceEndpointConfiguration>(),
                                                  new ReflectionDiscoverableCollection<IClientScript>(loggerMock.Object),
                                                  loggerMock,
                                                  new Mock<IHtmlEncoder>(),
                                                  new Mock<IPersistanceStore>(),
                                                  new ReflectionDiscoverableCollection<IPipelineInspector>(
                                                      loggerMock.Object),
                                                  new ReflectionDiscoverableCollection<IResource>(loggerMock.Object),
                                                  new Mock<ISerializer>(),
                                                  new ReflectionDiscoverableCollection<ITab>(loggerMock.Object),
                                                  new ReflectionDiscoverableCollection<IRuntimePolicy>(loggerMock.Object),
                                                  new Mock<IResource>(),
                                                  new Mock<IProxyFactory>(),
                                                  new Mock<IMessageBroker>());
        }

        public Mock<ResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        public Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
        public IDiscoverableCollection<IClientScript> ClientScriptsStub { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<IHtmlEncoder> HtmlEncoderMock { get; set; }
        public Mock<IPersistanceStore> PersistanceStoreMock { get; set; }
        public Mock<ISerializer> SerializerMock { get; set; }
    }
}