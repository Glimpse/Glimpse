using System;
using System.Runtime.Serialization;

namespace Glimpse.Core2
{
    public class GlimpseException:Exception
    {
        public GlimpseException()
        {
        }

        public GlimpseException(string message) : base(message)
        {
        }

        public GlimpseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GlimpseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}