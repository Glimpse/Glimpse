using System;

namespace Glimpse.WebForms.Warning
{
    internal class ExceptionWarning:Warning
    {
        public ExceptionWarning(Exception exception)
        {
            Message = exception.Message;
        }
    }
}
