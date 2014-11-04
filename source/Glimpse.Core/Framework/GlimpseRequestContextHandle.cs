using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// This is a handle that contains the necessary information to track a corresponding <see cref="IGlimpseRequestContext" /> and which will 
    /// make sure the <see cref="ActiveGlimpseRequestContexts"/> instance, who created this handle, will be notified when it is being disposed, 
    /// either explicitly or implicitly when the finalizer of this handle is run by the Garbage Collector.
    /// </summary>
    public class GlimpseRequestContextHandle : IDisposable
    {
        private bool Disposed { get; set; }
        
        private Action OnDisposeCallback { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRequestContextHandle"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        /// <param name="requestHandlingMode">Mode representing the way Glimpse is handling the request.</param>
        /// <param name="onDisposeCallback">The callback to be executed when this instance is being disposed.</param>
        internal GlimpseRequestContextHandle(Guid glimpseRequestId, RequestHandlingMode requestHandlingMode, Action onDisposeCallback)
        {
            if (onDisposeCallback == null)
            {
                throw new ArgumentNullException("onDisposeCallback");
            }

            GlimpseRequestId = glimpseRequestId;
            RequestHandlingMode = requestHandlingMode;
            OnDisposeCallback = onDisposeCallback;
        }

        /// <summary>
        /// Finalizes the instance
        /// </summary>
        ~GlimpseRequestContextHandle()
        {
            Dispose(false);
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
        /// Disposes the handle, which will make sure the <see cref="ActiveGlimpseRequestContexts"/> instance, who created this handle, will be notified.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the handle, which will make sure the <see cref="ActiveGlimpseRequestContexts"/> instance, who created this handle, will be notified.
        /// </summary>
        /// <param name="disposing">Boolean indicating whether this method is called from the public <see cref="Dispose()"/> method or from within the finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                }

                try
                {
                    OnDisposeCallback();
                    Disposed = true;
                }
                catch (Exception disposeException)
                {
                    GlimpseRuntime.Instance.Configuration.Logger.Error("Failed to dispose Glimpse request context handle", disposeException);
                }
            }
        }
    }
}