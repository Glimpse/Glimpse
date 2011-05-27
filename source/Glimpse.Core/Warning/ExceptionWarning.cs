using System;

namespace Glimpse.Core.Warning
{
    internal class ExceptionWarning:Warning
    {
        public ExceptionWarning(Exception exception)
        {
            Message = exception.Message;
        }
    }
}
