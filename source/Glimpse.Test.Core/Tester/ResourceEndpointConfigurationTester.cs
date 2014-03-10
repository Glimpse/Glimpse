using System;
using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class ResourceEndpointConfigurationTester : AjaxPolicy
    {
        public Mock<IResourceEndpointConfiguration> ResourceEndpointConfigurationMock { get; set; }

        private ResourceEndpointConfigurationTester(Uri requestUri, bool isResourceRequest)
        {
            const string endpointBaseUri = "/glimpse.axd";
            ResourceEndpointConfigurationMock = new Mock<IResourceEndpointConfiguration>();
            ResourceEndpointConfigurationMock
                .Setup(resourceEndpointConfiguration => resourceEndpointConfiguration.IsResourceRequest(requestUri, endpointBaseUri))
                .Returns(isResourceRequest);
        }

        public static ResourceEndpointConfigurationTester Create(Uri requestUri, bool isResourceRequest)
        {
            return new ResourceEndpointConfigurationTester(requestUri, isResourceRequest);
        }
    }
}