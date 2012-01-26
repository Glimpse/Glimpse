using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class StatusCodeResourceResult : IResourceResult
    {
        public int StatusCode { get; set; }
        public StatusCodeResourceResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public void Execute(IFrameworkProvider frameworkProvider)
        {
            frameworkProvider.SetHttpResponseStatusCode(StatusCode);
        }
    }
}