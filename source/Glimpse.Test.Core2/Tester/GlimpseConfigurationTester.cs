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
                                           Mock<IHtmlEncoder> htmlEncoderMock)
            : base(
                frameworkProviderMock.Object,
                endpointConfigurationMock.Object,
                clientScriptsStub,
                loggerMock.Object,
                RuntimePolicy.On,
                htmlEncoderMock.Object)
        {
            FrameworkProviderMock = frameworkProviderMock;
            EndpointConfigMock = endpointConfigurationMock;
            ClientScriptsStub = clientScriptsStub;
            LoggerMock = loggerMock;
            HtmlEncoderMock = htmlEncoderMock;
        }

        public static GlimpseConfigurationTester Create()
        {
            var loggerMock = new Mock<ILogger>();

            return new GlimpseConfigurationTester(new Mock<IFrameworkProvider>().Setup(),
                                                  new Mock<ResourceEndpointConfiguration>(),
                                                  new ReflectionDiscoverableCollection<IClientScript>(loggerMock.Object),
                                                  loggerMock,
                                                  new Mock<IHtmlEncoder>());
        }

        public Mock<ResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        public Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
        public IDiscoverableCollection<IClientScript> ClientScriptsStub { get; set; }
        public Mock<ILogger> LoggerMock { get; set; }
        public Mock<IHtmlEncoder> HtmlEncoderMock { get; set; }
    }
}