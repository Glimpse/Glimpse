using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class ResourceParametersTester : ResourceParameters
    {
        public Mock<IResource> ResourceMock { get; set; }
        
        private ResourceParametersTester(IDictionary<string,string> namedParams):base(namedParams)
        {
            ResourceMock = new Mock<IResource>();
            ResourceMock.Setup(r => r.Parameters).Returns(new[] {new ResourceParameterMetadata("One"), new ResourceParameterMetadata("Two"), new ResourceParameterMetadata("Three")});
        }

        public static ResourceParametersTester Create()
        {
            IDictionary<string, string> none = null;
            return new ResourceParametersTester(none);
        }
    }
}