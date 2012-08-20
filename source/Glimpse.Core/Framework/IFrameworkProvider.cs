using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    public interface IFrameworkProvider
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        IRequestMetadata RequestMetadata { get; }
        void SetHttpResponseHeader(string name, string value);
        void SetHttpResponseStatusCode(int statusCode);
        void SetCookie(string name, string value);
        void InjectHttpResponseBody(string htmlSnippet);
        void WriteHttpResponse(byte[] content);
        void WriteHttpResponse(string content);
    }
}
