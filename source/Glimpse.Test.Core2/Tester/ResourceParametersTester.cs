using System.Collections.Generic;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;

namespace Glimpse.Test.Core2.Tester
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