using System;
using System.Runtime.Remoting.Messaging;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Implementation of a <see cref="ICurrentGlimpseRequestIdTracker"/> that tracks a given Glimpse request Id inside the <see cref="CallContext"/>
    /// </summary>
    public class CallContextCurrentGlimpseRequestIdTracker : ICurrentGlimpseRequestIdTracker
    {
        protected const string RequestIdKey = "__GlimpseRequestIdTracker";

        /// <summary>
        /// Tracks the Glimpse request id inside the <see cref="CallContext"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request id to track.</param>
        public virtual void StartTracking(Guid glimpseRequestId)
        {
            CallContext.LogicalSetData(RequestIdKey, glimpseRequestId);
        }

        /// <summary>
        /// Tries to get the tracked Glimpse request id from the <see cref="CallContext"/>
        /// </summary>
        /// <param name="glimpseRequestId">The tracked Glimpse request id, or the default <see cref="Guid"/> in case it was not found</param>
        /// <returns>Boolean indicating whether a Glimpse request id was found or not.</returns>
        public virtual bool TryGet(out Guid glimpseRequestId)
        {
            glimpseRequestId = new Guid();

            var possibleGlimpseRequestId = CallContext.LogicalGetData(RequestIdKey) as Guid?;
            if (possibleGlimpseRequestId.HasValue)
            {
                glimpseRequestId = possibleGlimpseRequestId.Value;
            }

            return possibleGlimpseRequestId.HasValue;
        }

        /// <summary>
        /// Stops tracking the Glimpse request id inside the <see cref="CallContext"/> 
        /// </summary>
        public virtual void StopTracking()
        {
            CallContext.LogicalSetData(RequestIdKey, null);
            CallContext.FreeNamedDataSlot(RequestIdKey);
        }
    }
}