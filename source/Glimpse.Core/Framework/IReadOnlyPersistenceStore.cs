using System;
using System.Collections.Generic;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement a read only store that Glimpse can 
    /// use to retrieve requests and system metadata.
    /// </summary>
    public interface IReadOnlyPersistenceStore
    {
        /// <summary>
        /// Gets the by request id.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns>Instance of the request that matches the request id.</returns>
        GlimpseRequest GetByRequestId(Guid requestId);

        /// <summary>
        /// Gets the by request id and tab key.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="tabKey">The tab key.</param>
        /// <returns>Instance of the tab data that matches the request id and tab key.</returns>
        TabResult GetByRequestIdAndTabKey(Guid requestId, string tabKey);

        /// <summary>
        /// Gets the by request parent id.
        /// </summary>
        /// <param name="parentRequestId">The parent request id.</param>
        /// <returns>Collection of requests that matches the parent request id.</returns>
        IEnumerable<GlimpseRequest> GetByRequestParentId(Guid parentRequestId);

        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>Collection of requests that represent the top x number of requests.</returns>
        IEnumerable<GlimpseRequest> GetTop(int count);

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <returns>Metadata that is currently applied.</returns>
        IDictionary<string, object> GetMetadata();
    }
}