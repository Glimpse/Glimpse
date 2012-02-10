using System;

namespace Glimpse.Core2.Extensibility
{
    public class ExceptionResourceResult:IResourceResult
    {
        public Exception Exception { get; set; }

        public ExceptionResourceResult(Exception exception)
        {
            Exception = exception;
        }

        public void Execute(IResourceResultContext context)
        {
            context.FrameworkProvider.WriteHttpResponse(Exception.ToString());
            context.FrameworkProvider.SetHttpResponseStatusCode(500);
        }
    }
}