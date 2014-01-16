using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Tracks active <see cref="GlimpseRequestContext"/> instances
    /// </summary>
    public static class ActiveGlimpseRequestContexts
    {
        private static readonly Guid EmptyGuid = new Guid();
        private static IDictionary<Guid, GlimpseRequestContext> GlimpseRequestContexts { get; set; }

        /// <summary>
        /// Initializes the type <see cref="ActiveGlimpseRequestContexts"/>
        /// </summary>
        static ActiveGlimpseRequestContexts()
        {
            GlimpseRequestContexts = new Dictionary<Guid, GlimpseRequestContext>();
        }

        /// <summary>
        /// Adds the given <see cref="GlimpseRequestContext"/> to the list of active glimpse request contexts
        /// </summary>
        /// <param name="glimpseRequestContext">The <see cref="GlimpseRequestContext"/> to add</param>
        /// <returns>
        /// A <see cref="GlimpseRequestContextHandle"/> that will make sure the given <see cref="GlimpseRequestContext"/> is removed from
        /// the list of active Glimpse request contexts once it is disposed or finalized.
        /// </returns>
        public static GlimpseRequestContextHandle Add(GlimpseRequestContext glimpseRequestContext)
        {
            // at this point, the glimpseRequestContext isn't stored anywhere, but before we put it in the list of active glimpse requests contexts
            // we'll create the handle. This handle will make sure the glimpseRequestContext is removed from the collection of active glimpse request contexts
            // in case something goes wrong further on. That's is also why we create the handle first and then add the the glimpseRequestContext to the list
            // because if the creation of the handle would fail afterwards, then there is no way to remove the glimpseRequestContext from the list.

            var handle = new GlimpseRequestContextHandle(glimpseRequestContext.GlimpseRequestId);
            GlimpseRequestContexts.Add(glimpseRequestContext.GlimpseRequestId, glimpseRequestContext);

            // we also store the GlimpseRequestId in the CallContext for later use. That is our only entry point to retrieve the glimpseRequestContext
            // when we are not inside one of the GlimpseRuntime methods that is being provided with the requestResponseAdapter
            CallContext.LogicalSetData(Constants.RequestIdKey, glimpseRequestContext.GlimpseRequestId);

            return handle;
        }

        /// <summary>
        /// Tries to get the corresponding <see cref="GlimpseRequestContext" /> from the list of active Glimpse request contexts.
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse Id for which the corresponding <see cref="GlimpseRequestContext"/> must be returned</param>
        /// <param name="glimpseRequestContext">The corresponding <see cref="GlimpseRequestContext"/></param>
        /// <returns>Boolean indicating whether or not the corresponding <see cref="GlimpseRequestContext"/> was found.</returns>
        public static bool TryGet(Guid glimpseRequestId, out GlimpseRequestContext glimpseRequestContext)
        {
            return GlimpseRequestContexts.TryGetValue(glimpseRequestId, out glimpseRequestContext);
        }

        /// <summary>
        /// Removes the corresponding <see cref="GlimpseRequestContext" /> from the list of active Glimpse request contexts.
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse Id for which the corresponding <see cref="GlimpseRequestContext"/> must be removed</param>
        public static void Remove(Guid glimpseRequestId)
        {
            GlimpseRequestContexts.Remove(glimpseRequestId);
            CallContext.FreeNamedDataSlot(Constants.RequestIdKey);
        }

        /// <summary>
        /// Removes all the stored <see cref="GlimpseRequestContext"/> from the list of active Glimpse request contexts
        /// </summary>
        public static void RemoveAll()
        {
            GlimpseRequestContexts.Clear();
        }

        public static GlimpseRequestContext Current
        {
            get
            {
                Guid? glimpseRequestId = CallContext.LogicalGetData(Constants.RequestIdKey) as Guid?;
                if (!glimpseRequestId.HasValue)
                {
                    // there is no context registered, which means Glimpse did not initialize itself for this request aka GlimpseRuntime.BeginRequest has not been
                    // called even when there is code that wants to check this
                    // either way, we return here an empty context which indicates that Glimpse is disabled
                    return InactiveGlimpseRequestContext.Instance;
                }

                // we have a Glimpse Request Id, now we need to check whether we can find the corresponding GlimpseRequestContext
                GlimpseRequestContext glimpseRequestContext;
                if (TryGet(glimpseRequestId.Value, out glimpseRequestContext))
                {
                    return glimpseRequestContext;
                }

                // for some reason the context corresponding to the glimpse request id is not found
                throw new GlimpseException("No corresponding GlimpseRequestContext found for GlimpseRequestId '" + glimpseRequestId.Value + "'.");
            }
        }

        /// <summary>
        /// This handle will make sure the corresponding <see cref="GlimpseRequestContext" /> will be removed from the <see cref="ActiveGlimpseRequestContexts"/>.
        /// This will be done when this handle is explicitly disposed or when the finalizer of this handle is run by the GC.
        /// </summary>
        public class GlimpseRequestContextHandle : IDisposable
        {
            private bool disposed;

            /// <summary>
            /// Initializes a new instance of the <see cref="GlimpseRequestContextHandle"/>
            /// </summary>
            /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
            public GlimpseRequestContextHandle(Guid glimpseRequestId)
            {
                GlimpseRequestId = glimpseRequestId;
            }

            /// <summary>
            /// Finalizes the instance
            /// </summary>
            ~GlimpseRequestContextHandle()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// Gets the Glimpse Id assigned to this request
            /// </summary>
            public Guid GlimpseRequestId { get; private set; }

            /// <summary>
            /// Disposes the handle, which will make sure the corresponding <see cref="GlimpseRequestContext"/> is removed from the <see cref="ActiveGlimpseRequestContexts"/>
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                    }

                    ActiveGlimpseRequestContexts.Remove(GlimpseRequestId);
                    this.disposed = true;
                }
            }
        }
    }
}