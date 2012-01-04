using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2
{
    public class StatusCodeResourceResult : ResourceResult
    {
        public int StatusCode { get; set; }
        public StatusCodeResourceResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public override void Execute(IFrameworkProvider frameworkProvider)
        {
            frameworkProvider.SetHttpResponseStatusCode(StatusCode);
        }
    }
}