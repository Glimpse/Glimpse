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
            Mock<ILogger> loggerMock)
            : base(frameworkProviderMock.Object, endpointConfigurationMock.Object, clientScriptsStub, loggerMock.Object)
        {
            FrameworkProviderMock = frameworkProviderMock;
            EndpointConfigMock = endpointConfigurationMock;
            ClientScriptsStub = clientScriptsStub;
            LoggerMock = loggerMock;
        }

        public static GlimpseConfigurationTester Create()
        {
            return new GlimpseConfigurationTester(new Mock<IFrameworkProvider>().Setup(),
                                                  new Mock<ResourceEndpointConfiguration>(),
                                                  new ReflectionDiscoverableCollection<IClientScript>(new Mock<ILogger>().Object),
                                                  new Mock<ILogger>());
        }

        public Mock<ResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        public Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
        public IDiscoverableCollection<IClientScript> ClientScriptsStub { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
    }
}