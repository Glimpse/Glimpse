using System;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    /// <summary>
    /// Represents a <see cref="ICurrentGlimpseRequestIdTracker"/> tracker that uses the <see cref="HttpContext.Current"/> Items collection
    /// if available, while falling back onto the <see cref="CallContext"/> in case it isn't
    /// </summary>
    internal class AspNetCurrentGlimpseRequestIdTracker : CallContextCurrentGlimpseRequestIdTracker
    {
        /// <summary>
        /// Tracks the Glimpse request id inside the <see cref="HttpContext.Current"/> items collection if available while
        /// falling back onto the <see cref="CallContext"/> in case it isn't
        /// <see cref="CallContext"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request id to track.</param>
        public override void StartTracking(Guid glimpseRequestId)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items[RequestIdKey] = glimpseRequestId;
            }

            // we still track inside the CallContext, so that thread switches where the HttpContext.Current isn't 
            // transferred can still find the Glimpse request id
            base.StartTracking(glimpseRequestId);
        }

        /// <summary>
        /// Tries to get the tracked Glimpse request id from the <see cref="HttpContext.Current"/> Items collection
        /// if available, while falling back onto the <see cref="CallContext"/> in case it isn't<see cref="CallContext"/>
        /// </summary>
        /// <param name="glimpseRequestId">The tracked Glimpse request id, or the default <see cref="Guid"/> in case it was not found</param>
        /// <returns>Boolean indicating whether a Glimpse request id was found or not.</returns>
        public override bool TryGet(out Guid glimpseRequestId)
        {
            if (HttpContext.Current != null)
            {
                var possibleGlimpseRequestId = HttpContext.Current.Items[RequestIdKey] as Guid?;
                if (possibleGlimpseRequestId.HasValue)
                {
                    glimpseRequestId = possibleGlimpseRequestId.Value;

                    // It is perfectly possible that the Glimpse request id can be found in the HttpContext.Current.Items collection and
                    // not in the CallContext, in that case we'll add if for future use. 
                    // What is not normal and even wrong, is that the CallContext returns another Glimpse request id then the one stored inside
                    // the HttpContext.Current.Items collection, so we're going to check this to make sure and log an error in case we find ourselves
                    // in such a situation
                    Guid callcontextStoredGlimpseRequestId;
                    if (base.TryGet(out callcontextStoredGlimpseRequestId) && callcontextStoredGlimpseRequestId != glimpseRequestId)
                    {
                        if (GlimpseRuntime.IsAvailable)
                        {
                            GlimpseRuntime.Instance.Configuration.Logger.Error("Glimpse request id '" + callcontextStoredGlimpseRequestId + "' was found in the CallContext but it differs from the one found in the HttpContext.Current.Items collection being '" + glimpseRequestId + "'.");
                        }

                        // so the stored Glimpse request id was wrong, so we'll overwrite it with the correct one
                        base.StartTracking(glimpseRequestId);
                    }

                    return true;
                }
            }

            return base.TryGet(out glimpseRequestId);
        }

        /// <summary>
        /// Stops tracking the Glimpse request id inside the <see cref="HttpContext.Current"/> Items collection
        /// if available, while falling back onto the <see cref="CallContext"/> in case it isn't
        /// </summary>
        public override void StopTracking()
        {
            base.StopTracking();
            HttpContext.Current.Items.Remove(RequestIdKey);
        }
    }
}