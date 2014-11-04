using System;
using System.Collections.Generic;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;

namespace Glimpse.Test.Core.Tester
{
    public class InMemoryPersistenceStoreTester : InMemoryPersistenceStore
    {
        public Mock<IRequestMetadata> RequestMetadataMock { get; set; }

        private InMemoryPersistenceStoreTester(IDataStore dataStore):base(dataStore)
        {
            RequestMetadataMock = new Mock<IRequestMetadata>();
            RequestMetadataMock.Setup(r => r.RequestHttpMethod).Returns("POST");
            RequestMetadataMock.Setup(r => r.RequestUri).Returns(new Uri("http://localhost"));
            RequestMetadataMock.Setup(r => r.GetHttpHeader(Constants.HttpRequestHeader)).Returns("936DA01F-9ABD-4d9d-80C7-02AF85C822A8");
            RequestMetadataMock.Setup(r => r.RequestIsAjax).Returns(true);
        }

        public static InMemoryPersistenceStoreTester Create()
        {
            return new InMemoryPersistenceStoreTester(new DictionaryDataStoreAdapter(new Dictionary<object, object>()));
        }
    }
}