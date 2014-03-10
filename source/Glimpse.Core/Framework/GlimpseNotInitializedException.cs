using System;
using System.Runtime.Serialization;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// A custom exception thrown for unrecoverable Glimpse issues.
    /// </summary>
    public class GlimpseNotInitializedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseNotInitializedException" /> class.
        /// </summary>
        public GlimpseNotInitializedException()
            : this("GlimpseRuntime has not (yet) been initialized, make sure to call GlimpseRuntime.Initialize before accessing the singleton instance.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseNotInitializedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GlimpseNotInitializedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseNotInitializedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlimpseNotInitializedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseNotInitializedException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public GlimpseNotInitializedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}