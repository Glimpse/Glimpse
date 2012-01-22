using System.Collections.Generic;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Moq;

namespace Glimpse.Test.Core2.Tester
{
    public class ApplicationPersistanceStoreTester : ApplicationPersistanceStore
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }

        private ApplicationPersistanceStoreTester(IDataStore dataStore):base(dataStore)
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.RequestHttpMethod).Returns("POST");
            RequestMetadataMock.Setup(r => r.RequestUri).Returns("http://localhost");
            RequestMetadataMock.Setup(r => r.GetHttpHeader(Constants.HttpRequestHeader)).Returns("936DA01F-9ABD-4d9d-80C7-02AF85C822A8");
            RequestMetadataMock.Setup(r => r.RequestIsAjax).Returns(true);
        }

        public static ApplicationPersistanceStoreTester Create()
        {
            return new ApplicationPersistanceStoreTester(new DictionaryDataStoreAdapter(new Dictionary<object, object>()));
        }
    }
}