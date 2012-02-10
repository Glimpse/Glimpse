using System;

namespace Glimpse.Core2.Extensibility
{
    public class ExceptionResourceResult : StatusCodeResourceResult
    {
        public ExceptionResourceResult(Exception exception):base(500, exception.ToString()){}
    }
}