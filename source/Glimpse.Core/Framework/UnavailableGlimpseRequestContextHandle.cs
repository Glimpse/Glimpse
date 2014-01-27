using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// This handle will make sure the corresponding <see cref="GlimpseRequestContext" /> will be removed from the <see cref="ActiveGlimpseRequestContexts"/>.
    /// This will be done when this handle is explicitly disposed or when the finalizer of this handle is run by the Garbage Collector.
    /// </summary>
    public sealed class UnavailableGlimpseRequestContextHandle : GlimpseRequestContextHandle
    {
        /// <summary>
        /// Represents a <see cref="GlimpseRequestContextHandle"/> in case Glimpse is disabled.
        /// </summary>
        public static readonly UnavailableGlimpseRequestContextHandle Instance = new UnavailableGlimpseRequestContextHandle();

        /// <summary>
        /// Initializes a new instance of the <see cref="UnavailableGlimpseRequestContextHandle"/>
        /// </summary>
        private UnavailableGlimpseRequestContextHandle()
            : base(new Guid(), RequestHandlingMode.Unhandled)
        {
        }

        /// <summary>
        /// Disposes the handle, which in effect will do nothing as this handles does not track a <see cref="GlimpseRequestContext"/>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // no cleanup needs to be done, as this handle does not reference a GlimpseRequestContext instance
        }
    }
}