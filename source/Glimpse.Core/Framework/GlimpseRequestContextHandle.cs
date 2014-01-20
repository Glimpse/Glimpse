using System;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// This handle will make sure the corresponding <see cref="GlimpseRequestContext" /> will be removed from the <see cref="ActiveGlimpseRequestContexts"/>.
    /// This will be done when this handle is explicitly disposed or when the finalizer of this handle is run by the Garbage Collector.
    /// </summary>
    public class GlimpseRequestContextHandle : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContextHandle"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        internal GlimpseRequestContextHandle(Guid glimpseRequestId)
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