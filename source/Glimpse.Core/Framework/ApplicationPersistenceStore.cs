using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// An <see cref="IPersistenceStore"/> which stores Glimpse request and configuration data in application store.
    /// </summary>
    /// <remarks>
    /// An example of an application store is <c>HttpContext.Current.Application</c> in ASP.NET.
    /// </remarks>
    public class ApplicationPersistenceStore : IPersistenceStore
    {
        private const string PersistenceStoreKey = "__GlimpsePersistenceKey";
                
        private readonly object queueLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPersistenceStore" /> class.
        /// </summary>
        /// <param name="dataStore">The data store.</param>
        public ApplicationPersistenceStore(IDataStore dataStore, int bufferSize)
        {
            DataStore = dataStore;
            BufferSize = bufferSize;

            var glimpseRequests = DataStore.Get<Queue<GlimpseRequest>>(PersistenceStoreKey);
            if (glimpseRequests == null)
            {
                glimpseRequests = new Queue<GlimpseRequest>(BufferSize);
                DataStore.Set(PersistenceStoreKey, glimpseRequests);
            }

            GlimpseRequests = glimpseRequests;
        }

        internal Queue<GlimpseRequest> GlimpseRequests { get; set; }

        private IDataStore DataStore { get; set; }

        private GlimpseMetadata Metadata { get; set; }

        private int BufferSize { get; set; }

        /// <summary>
        /// Saves the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Save(GlimpseRequest request)
        {
            lock (queueLock)
            {
                if (GlimpseRequests.Count >= BufferSize)
                {
                    GlimpseRequests.Dequeue();
                }

                GlimpseRequests.Enqueue(request);
            }
        }

        /// <summary>
        /// Saves the specified system metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void Save(GlimpseMetadata metadata)
        {
            Metadata = metadata;
        }

        /// <summary>
        /// Gets the by request id.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns>
        /// Instance of the request that matches the request id.
        /// </returns>
        public GlimpseRequest GetByRequestId(Guid requestId)
        {
            lock (queueLock)
            {
                return GlimpseRequests.FirstOrDefault(r => r.RequestId == requestId);
            }
        }

        /// <summary>
        /// Gets the by request id and tab key.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="tabKey">The tab key.</param>
        /// <returns>
        /// Instance of the tab data that matches the request id and tab key.
        /// </returns>
        /// <exception cref="System.ArgumentException">Throws an exception if <paramref name="tabKey"/> is <c>null</c>.</exception>
        public TabResult GetByRequestIdAndTabKey(Guid requestId, string tabKey)
        {
            if (string.IsNullOrEmpty(tabKey))
            {
                throw new ArgumentException("tabKey");
            }

            var request = GetByRequestId(requestId);

            if (request == null || !request.TabData.ContainsKey(tabKey))
            {
                return null;
            }

            return request.TabData[tabKey];
        }

        /// <summary>
        /// Gets the by request parent id.
        /// </summary>
        /// <param name="parentRequestId">The parent request id.</param>
        /// <returns>
        /// Collection of requests that matches the parent request id.
        /// </returns>
        public IEnumerable<GlimpseRequest> GetByRequestParentId(Guid parentRequestId)
        {
            lock (queueLock)
            {
                return GlimpseRequests.Where(r => r.ParentRequestId == parentRequestId).ToList();
            }
        }

        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>
        /// Collection of requests that represent the top x number of requests.
        /// </returns>
        public IEnumerable<GlimpseRequest> GetTop(int count)
        {
            lock (queueLock)
            {
                return GlimpseRequests.Take(count).ToList();
            }
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns>
        /// Metadata that is currently applied.
        /// </returns>
        public GlimpseMetadata GetMetadata()
        {
            return Metadata;
        }
    }
}