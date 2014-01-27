using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Policy;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class RequestResponseAdapterTester : AjaxPolicy
    {
        public Mock<IRequestResponseAdapter> RequestResponseAdapterMock { get; set; }

        private RequestResponseAdapterTester(RuntimePolicy initialRuntimePolicy, string requestAbsolutePath)
        {
            var requestStore = new DictionaryDataStoreAdapter(new Dictionary<string, object>
            {
                { Constants.RuntimePolicyKey, initialRuntimePolicy }
            });

            var requestMetadata = new Mock<IRequestMetadata>();
            requestMetadata.Setup(metadata => metadata.AbsolutePath).Returns(requestAbsolutePath);

            RequestResponseAdapterMock = new Mock<IRequestResponseAdapter>();
            RequestResponseAdapterMock.Setup(adapter => adapter.HttpRequestStore).Returns(requestStore);
            RequestResponseAdapterMock.Setup(adapter => adapter.RequestMetadata).Returns(requestMetadata.Object);
        }

        public static RequestResponseAdapterTester Create(RuntimePolicy initialRuntimePolicy, string requestAbsolutePath)
        {
            return new RequestResponseAdapterTester(initialRuntimePolicy, requestAbsolutePath);
        }
    }
}