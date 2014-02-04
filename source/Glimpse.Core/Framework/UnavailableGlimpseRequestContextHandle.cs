using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents a handle in case there is no <see cref="IGlimpseRequestContext"/> available.
    /// </summary>
    public sealed class UnavailableGlimpseRequestContextHandle : GlimpseRequestContextHandle
    {
        /// <summary>
        /// Represents a <see cref="GlimpseRequestContextHandle"/> in case there is no <see cref="IGlimpseRequestContext"/> available.
        /// </summary>
        public static UnavailableGlimpseRequestContextHandle Instance { get; private set; }

        static UnavailableGlimpseRequestContextHandle()
        {
            Instance = new UnavailableGlimpseRequestContextHandle();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableGlimpseRequestContextHandle"/>
        /// </summary>
        private UnavailableGlimpseRequestContextHandle()
            : base(new Guid(), RequestHandlingMode.Unhandled)
        {
        }

        /// <summary>
        /// Disposes the handle, which in effect will do nothing as this handle is only used in case there is no <see cref="IGlimpseRequestContext"/> available
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // no cleanup needs to be done, as this handle does not reference a GlimpseRequestContext instance
        }
    }
}