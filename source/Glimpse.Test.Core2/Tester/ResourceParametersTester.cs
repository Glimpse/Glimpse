using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class ResourceParametersTester : ResourceParameters
    {
        public Mock<IResource> ResourceMock { get; set; }
        
        private ResourceParametersTester()
        {
            ResourceMock = new Mock<IResource>();
            ResourceMock.Setup(r => r.ParameterKeys).Returns(new[] {"One", "Two", "Three"});
        }

        public static ResourceParametersTester Create()
        {
            return new ResourceParametersTester();
        }
    }
}