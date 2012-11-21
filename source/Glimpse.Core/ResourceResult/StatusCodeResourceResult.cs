using Glimpse.Core.Extensibility;

namespace Glimpse.Core.ResourceResult
{
    public class StatusCodeResourceResult : IResourceResult
    {
        public StatusCodeResourceResult(int statusCode) : this(statusCode, string.Empty)
        {
        }

        public StatusCodeResourceResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public void Execute(IResourceResultContext context)
        {
            var frameworkProvider = context.FrameworkProvider;

            frameworkProvider.WriteHttpResponse(Message);
            frameworkProvider.SetHttpResponseStatusCode(StatusCode);
        }
    }
}