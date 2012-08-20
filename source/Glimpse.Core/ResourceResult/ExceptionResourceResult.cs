using System;

namespace Glimpse.Core.ResourceResult
{
    public class ExceptionResourceResult : StatusCodeResourceResult
    {
        public ExceptionResourceResult(Exception exception):base(500, exception.ToString()){}
    }
}