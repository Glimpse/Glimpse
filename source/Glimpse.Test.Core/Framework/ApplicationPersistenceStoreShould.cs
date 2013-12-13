using System;
using System.Collections.Generic;
using System.Threading;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    public class ApplicationPersistenceStoreShould
    {
        [Fact]
        public void BeThreadSafe()
        {
            var sut = new ApplicationPersistenceStore(new DictionaryDataStoreAdapter(new Dictionary<string, object>()));

            Action<ApplicationPersistenceStore> addingRequests = store =>
            {
                var glimpseRequest = new GlimpseRequest(
                    Guid.NewGuid(),
                    new Mock<IRequestMetadata>().Object,
                    new Dictionary<string, TabResult>(),
                    new Dictionary<string, TabResult>(),
                    new TimeSpan(1000));

                for (int requestCounter = 0; requestCounter < 200; requestCounter++)
                {
                    store.Save(glimpseRequest);

                    Thread.Sleep(10);
                }
            };

            Action<ApplicationPersistenceStore> gettingRequests = store =>
            {
                for (int requestCounter = 0; requestCounter < 200; requestCounter++)
                {
                    // gets will never by found with the given GUID, but that is not a problem, it's even a good
                    // thing for this test, since it will enumerate the complete collection, quicker running into
                    // threading issues while the state is being manipulated while enumerating it.

                    store.GetByRequestId(Guid.NewGuid());
                    store.GetByRequestParentId(Guid.NewGuid());
                    store.GetByRequestIdAndTabKey(Guid.NewGuid(), "SomeUnknownTabKey");
                    store.GetTop(10);

                    Thread.Sleep(14);
                }
            };

            var invokedDelegates = new List<Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>>
            {
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(addingRequests, addingRequests.BeginInvoke(sut, null, null)),
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(gettingRequests, gettingRequests.BeginInvoke(sut, null, null)),
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(addingRequests, addingRequests.BeginInvoke(sut, null, null)),
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(gettingRequests, gettingRequests.BeginInvoke(sut, null, null)),
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(addingRequests, addingRequests.BeginInvoke(sut, null, null)),
                new Tuple<Action<ApplicationPersistenceStore>, IAsyncResult>(gettingRequests, gettingRequests.BeginInvoke(sut, null, null))
            };

            foreach (var invokedDelegate in invokedDelegates)
            {
                invokedDelegate.Item1.EndInvoke(invokedDelegate.Item2);
            }
        }
    }
}