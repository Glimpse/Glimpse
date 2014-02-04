using System;
using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class RequestResponseAdapterTester : AjaxPolicy
    {
        public Mock<IRequestResponseAdapter> RequestResponseAdapterMock { get; set; }

        private RequestResponseAdapterTester(Uri requestUri)
        {
            var requestMetadata = new Mock<IRequestMetadata>();
            requestMetadata.Setup(metadata => metadata.RequestUri).Returns(requestUri);

            RequestResponseAdapterMock = new Mock<IRequestResponseAdapter>();
            RequestResponseAdapterMock.Setup(adapter => adapter.RequestMetadata).Returns(requestMetadata.Object);
        }

        public static RequestResponseAdapterTester Create(Uri requestUri)
        {
            return new RequestResponseAdapterTester(requestUri);
        }
    }
}