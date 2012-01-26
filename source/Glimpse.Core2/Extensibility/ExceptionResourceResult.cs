using System;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class ExceptionResourceResult:IResourceResult
    {
        public Exception Exception { get; set; }

        public ExceptionResourceResult(Exception exception)
        {
            Exception = exception;
        }

        public void Execute(IFrameworkProvider frameworkProvider)
        {
            frameworkProvider.SetHttpResponseStatusCode(500);
            //TODO: Add exception message to response
        }
    }
}