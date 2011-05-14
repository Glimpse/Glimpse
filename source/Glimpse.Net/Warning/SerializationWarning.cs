using System;

namespace Glimpse.Net.Warning
{
    internal class SerializationWarning:ExceptionWarning
    {
        public SerializationWarning(Exception exception) : base(exception)
        {
        }
    }
}
