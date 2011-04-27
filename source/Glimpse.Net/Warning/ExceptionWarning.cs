using System;

namespace Glimpse.Net.Warning
{
    internal class ExceptionWarning:Warning
    {
        public ExceptionWarning(Exception exception)
        {
            Message = exception.Message;
        }
    }
}
