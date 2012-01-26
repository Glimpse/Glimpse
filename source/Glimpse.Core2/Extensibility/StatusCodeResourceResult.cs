namespace Glimpse.Core2.Extensibility
{
    public class StatusCodeResourceResult : IResourceResult
    {
        public int StatusCode { get; set; }
        public StatusCodeResourceResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public void Execute(IResourceResultContext context)
        {
            context.FrameworkProvider.SetHttpResponseStatusCode(StatusCode);
        }
    }
}