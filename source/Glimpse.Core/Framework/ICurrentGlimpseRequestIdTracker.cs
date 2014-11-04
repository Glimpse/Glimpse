using System;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a store that keeps track of the Glimpse request id while a request is being processed.
    /// The store needs to make sure the request id is still available when threads are being switched
    /// while the request is being handled.
    /// </summary>
    public interface ICurrentGlimpseRequestIdTracker
    {
        /// <summary>
        /// Tracks the Glimpse request id while the request is being handled
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request id assigned to the request that is being handled.</param>
        void StartTracking(Guid glimpseRequestId);

        /// <summary>
        /// Tries to get the tracked Glimpse request id for the request that is currently being handled
        /// </summary>
        /// <param name="glimpseRequestId">The tracked Glimpse request id, or the default <see cref="Guid"/> in case it was not found</param>
        /// <returns>Boolean indicating whether a Glimpse request id was found or not.</returns>
        bool TryGet(out Guid glimpseRequestId);

        /// <summary>
        /// Stops tracking the Glimpse request id of the request that finished being handled. 
        /// </summary>
        void StopTracking();
    }
}