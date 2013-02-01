using System;
using System.Runtime.Serialization;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// A custom exception thrown for unrecoverable Glimpse issues.
    /// </summary>
    public class GlimpseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseException" /> class.
        /// </summary>
        public GlimpseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GlimpseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GlimpseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlimpseException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public GlimpseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}