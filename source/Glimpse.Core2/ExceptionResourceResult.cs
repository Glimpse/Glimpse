using System;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2
{
    public class ExceptionResourceResult:ResourceResult
    {
        public Exception Exception { get; set; }

        public ExceptionResourceResult(Exception exception)
        {
            Exception = exception;
        }

        public override void Execute(IFrameworkProvider frameworkProvider)
        {
            frameworkProvider.SetHttpResponseStatusCode(500);
            //TODO: Add exception message to response
        }
    }
}