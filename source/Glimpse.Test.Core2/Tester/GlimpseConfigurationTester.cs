using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Extensions;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class GlimpseConfigurationTester : GlimpseConfiguration
    {
        private GlimpseConfigurationTester(Mock<IFrameworkProvider> frameworkProviderMock,
                                           Mock<IResourceEndpointConfiguration> endpointConfigurationMock)
            : base(frameworkProviderMock.Object, endpointConfigurationMock.Object)
        {
            FrameworkProviderMock = frameworkProviderMock;
            EndpointConfigMock = endpointConfigurationMock;
        }

        public static GlimpseConfigurationTester Create()
        {
            return new GlimpseConfigurationTester(new Mock<IFrameworkProvider>().Setup(),
                                                  new Mock<IResourceEndpointConfiguration>());
        }

        public Mock<IResourceEndpointConfiguration> EndpointConfigMock { get; set; }
        public Mock<IFrameworkProvider> FrameworkProviderMock { get; set; }
    }
}