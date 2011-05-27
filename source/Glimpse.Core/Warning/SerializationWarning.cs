using System;

namespace Glimpse.Core.Warning
{
    internal class SerializationWarning:ExceptionWarning
    {
        public SerializationWarning(Exception exception) : base(exception)
        {
        }
    }
}
