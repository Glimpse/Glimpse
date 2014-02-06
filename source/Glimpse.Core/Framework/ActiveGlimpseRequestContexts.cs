using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Tracks active <see cref="IGlimpseRequestContext"/> instances
    /// </summary>
    internal class ActiveGlimpseRequestContexts
    {
        private static readonly object glimpseRequestContextsAccessLock = new object();
        private IDictionary<Guid, IGlimpseRequestContext> GlimpseRequestContexts { get; set; }
        private ICurrentGlimpseRequestIdTracker CurrentGlimpseRequestIdTracker { get; set; }

        /// <summary>
        /// Raised when a new <see cref="IGlimpseRequestContext"/> was added to the list of active Glimpse request contexts
        /// </summary>
        public static event EventHandler<ActiveGlimpseRequestContextEventArgs> RequestContextAdded = delegate { };

        /// <summary>
        /// Raised when an active <see cref="IGlimpseRequestContext"/> was removed from the list of active Glimpse request contexts
        /// </summary>
        public static event EventHandler<ActiveGlimpseRequestContextEventArgs> RequestContextRemoved = delegate { };

        /// <summary>
        /// Initializes the type <see cref="ActiveGlimpseRequestContexts"/>
        /// <param name="currentGlimpseRequestIdTracker">The <see cref="ICurrentGlimpseRequestIdTracker"/> to use</param>
        /// </summary>
        public ActiveGlimpseRequestContexts(ICurrentGlimpseRequestIdTracker currentGlimpseRequestIdTracker)
        {
            if (currentGlimpseRequestIdTracker == null)
            {
                throw new ArgumentNullException("currentGlimpseRequestIdTracker");
            }

            CurrentGlimpseRequestIdTracker = currentGlimpseRequestIdTracker;
            GlimpseRequestContexts = new Dictionary<Guid, IGlimpseRequestContext>();
        }

        /// <summary>
        /// Adds the given <see cref="IGlimpseRequestContext"/> to the list of active Glimpse request contexts
        /// </summary>
        /// <param name="glimpseRequestContext">The <see cref="IGlimpseRequestContext"/> to add</param>
        /// <returns>
        /// A <see cref="GlimpseRequestContextHandle"/> that will make sure the given <see cref="IGlimpseRequestContext"/> is removed from
        /// the list of active Glimpse request contexts once it is disposed or finalized.
        /// </returns>
        public GlimpseRequestContextHandle Add(IGlimpseRequestContext glimpseRequestContext)
        {
            // at this point, the glimpseRequestContext isn't stored anywhere, but before we put it in the list of active glimpse requests contexts
            // we'll create the handle. This handle will make sure the glimpseRequestContext is removed from the collection of active glimpse request contexts
            // in case something goes wrong further on. That's is also why we create the handle first and then add the the glimpseRequestContext to the list
            // because if the creation of the handle would fail afterwards, then there is no way to remove the glimpseRequestContext from the list.

            var glimpseRequestId = glimpseRequestContext.GlimpseRequestId;
            var handle = new GlimpseRequestContextHandle(glimpseRequestId, glimpseRequestContext.RequestHandlingMode, () => Remove(glimpseRequestId));
            lock (glimpseRequestContextsAccessLock)
            {
                /* 
                 * if we don't lock, then it is possible to get the following exception under heavy load:
                 * [IndexOutOfRangeException: Index was outside the bounds of the array.]
                 *    System.Collections.Generic.Dictionary`2.Resize(Int32 newSize, Boolean forceNewHashCodes)
                 *    System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
                 *    System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
                 */
                GlimpseRequestContexts.Add(glimpseRequestId, glimpseRequestContext);
            }

            CurrentGlimpseRequestIdTracker.StartTracking(glimpseRequestId);

            RaiseEvent(() => RequestContextAdded(this, new ActiveGlimpseRequestContextEventArgs(glimpseRequestId)), "RequestContextAdded");

            return handle;
        }

        /// <summary>
        /// Tries to get the corresponding <see cref="IGlimpseRequestContext" /> from the list of active Glimpse request contexts.
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse Id for which the corresponding <see cref="IGlimpseRequestContext"/> must be returned</param>
        /// <param name="glimpseRequestContext">The corresponding <see cref="IGlimpseRequestContext"/></param>
        /// <returns>Boolean indicating whether or not the corresponding <see cref="IGlimpseRequestContext"/> was found.</returns>
        public bool TryGet(Guid glimpseRequestId, out IGlimpseRequestContext glimpseRequestContext)
        {
            return GlimpseRequestContexts.TryGetValue(glimpseRequestId, out glimpseRequestContext);
        }

        /// <summary>
        /// Removes the corresponding <see cref="IGlimpseRequestContext" /> from the list of active Glimpse request contexts.
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse Id for which the corresponding <see cref="IGlimpseRequestContext"/> must be removed</param>
        private void Remove(Guid glimpseRequestId)
        {
            bool glimpseRequestContextRemoved;
            lock (glimpseRequestContextsAccessLock)
            {
                glimpseRequestContextRemoved = GlimpseRequestContexts.Remove(glimpseRequestId);
            }

            CurrentGlimpseRequestIdTracker.StopTracking();

            if (glimpseRequestContextRemoved)
            {
                RaiseEvent(() => RequestContextRemoved(this, new ActiveGlimpseRequestContextEventArgs(glimpseRequestId)), "RequestContextRemoved");
            }
        }

        /// <summary>
        /// Gets the current <see cref="IGlimpseRequestContext" /> based on the <see cref="CallContext"/>. If the <see cref="CallContext"/> has no matching
        /// Glimpse request Id, then an <see cref="UnavailableGlimpseRequestContext"/> will be returned instead. If the <see cref="CallContext"/> has a matching
        /// Glimpse request Id, but there is no corresponding <see cref="IGlimpseRequestContext"/> in the list of active Glimpse request contexts, then a
        /// <see cref="GlimpseException"/> is thrown.
        /// </summary>
        public IGlimpseRequestContext Current
        {
            get
            {
                Guid glimpseRequestId;
                if (!CurrentGlimpseRequestIdTracker.TryGet(out glimpseRequestId))
                {
                    if (GlimpseRuntime.IsInitialized)
                    {
                        GlimpseRuntime.Instance.Configuration.Logger.Warn("Returning UnavailableGlimpseRequestContext.Instance which is unexpected. If you set the log level to Trace, then you'll see the stack trace as well.");
                        GlimpseRuntime.Instance.Configuration.Logger.Trace("Call for UnavailableGlimpseRequestContext.Instance made from" + Environment.NewLine + "\t" + new StackTrace());
                    }

                    // there is no context registered, which means Glimpse did not initialize itself for this request aka GlimpseRuntime.BeginRequest has not been
                    // called even when there is code that wants to check this. Either way, we return here an empty context which indicates that Glimpse is disabled
                    return UnavailableGlimpseRequestContext.Instance;
                }

                // we have a Glimpse Request Id, now we need to check whether we can find the corresponding Glimpse request context
                IGlimpseRequestContext glimpseRequestContext;
                if (TryGet(glimpseRequestId, out glimpseRequestContext))
                {
                    return glimpseRequestContext;
                }

                // for some reason the context corresponding to the glimpse request id is not found
                throw new GlimpseException("No corresponding Glimpse request context found for GlimpseRequestId '" + glimpseRequestId + "'.");
            }
        }

        private static void RaiseEvent(Action eventRaiser, string eventName)
        {
            // we don't want any event handling code that throws an exception to have an impact on the workings of the ActiveGlimpseRequestContexts class
            try
            {
                eventRaiser();
            }
            catch (Exception exception)
            {
                if (GlimpseRuntime.IsInitialized)
                {
                    GlimpseRuntime.Instance.Configuration.Logger.Error("Exception occurred when '" + eventName + "' event got raised", exception);
                }
            }
        }
    }
}