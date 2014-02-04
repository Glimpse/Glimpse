using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// This handle will make sure the corresponding <see cref="IGlimpseRequestContext" /> will be removed from the <see cref="ActiveGlimpseRequestContexts"/>.
    /// This will be done when this handle is explicitly disposed or when the finalizer of this handle is run by the Garbage Collector.
    /// </summary>
    public class GlimpseRequestContextHandle : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContextHandle"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        /// <param name="requestHandlingMode">Mode representing the way Glimpse is handling the request.</param>
        internal GlimpseRequestContextHandle(Guid glimpseRequestId, RequestHandlingMode requestHandlingMode)
        {
            GlimpseRequestId = glimpseRequestId;
            RequestHandlingMode = requestHandlingMode;
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
        /// Gets the mode indicating how Glimpse is handling this request
        /// </summary>
        public RequestHandlingMode RequestHandlingMode { get; private set; }

        /// <summary>
        /// Disposes the handle, which will make sure the corresponding <see cref="IGlimpseRequestContext"/> is removed from the <see cref="ActiveGlimpseRequestContexts"/>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the handle, which will make sure the corresponding <see cref="IGlimpseRequestContext"/> is removed from the <see cref="ActiveGlimpseRequestContexts"/>
        /// </summary>
        /// <param name="disposing">Boolean indicating whether this method is called from the public <see cref="Dispose()"/> method or from within the finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                try
                {
                    ActiveGlimpseRequestContexts.Remove(GlimpseRequestId);
                    this.disposed = true;
                }
                catch (Exception disposeException)
                {
                    GlimpseRuntime.Instance.Configuration.Logger.Error("Failed to dispose Glimpse request context handle", disposeException);
                }
            }
        }
    }
}