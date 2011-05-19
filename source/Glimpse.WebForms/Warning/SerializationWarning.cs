using System;

namespace Glimpse.WebForms.Warning
{
    internal class SerializationWarning:ExceptionWarning
    {
        public SerializationWarning(Exception exception) : base(exception)
        {
        }
    }
}
