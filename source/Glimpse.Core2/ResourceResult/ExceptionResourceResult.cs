using System;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.ResourceResult
{
    public class ExceptionResourceResult : StatusCodeResourceResult
    {
        public ExceptionResourceResult(Exception exception):base(500, exception.ToString()){}
    }
}