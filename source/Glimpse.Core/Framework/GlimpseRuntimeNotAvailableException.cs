using System;
using System.Runtime.Serialization;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Exception thrown when the GlimpseRuntime instance is being accessed prematurely.
    /// </summary>
    public class GlimpseRuntimeNotAvailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRuntimeNotAvailableException" /> class.
        /// </summary>
        public GlimpseRuntimeNotAvailableException()
            : this("The GlimpseRuntime is not (yet) available, make sure to check the GlimpseRuntime.IsAvailable property before accessing the GlimpseRuntime.Instance . The GlimpseRuntime.Instance will be made available once the Glimpse runtime is initialized by calling GlimpseRuntime.Initializer.Initialize(...)")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRuntimeNotAvailableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GlimpseRuntimeNotAvailableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRuntimeNotAvailableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlimpseRuntimeNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseRuntimeNotAvailableException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public GlimpseRuntimeNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}