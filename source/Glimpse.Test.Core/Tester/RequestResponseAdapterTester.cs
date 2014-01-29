using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class RequestResponseAdapterTester : AjaxPolicy
    {
        public Mock<IRequestResponseAdapter> RequestResponseAdapterMock { get; set; }

        private RequestResponseAdapterTester(string requestAbsolutePath)
        {
            var requestMetadata = new Mock<IRequestMetadata>();
            requestMetadata.Setup(metadata => metadata.AbsolutePath).Returns(requestAbsolutePath);

            RequestResponseAdapterMock = new Mock<IRequestResponseAdapter>();
            RequestResponseAdapterMock.Setup(adapter => adapter.RequestMetadata).Returns(requestMetadata.Object);
        }

        public static RequestResponseAdapterTester Create(string requestAbsolutePath)
        {
            return new RequestResponseAdapterTester(requestAbsolutePath);
        }
    }
}