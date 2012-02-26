using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    //TODO: Does Http* make sense for names
    public interface IFrameworkProvider
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        //TODO: Do we really need this?
        IRequestMetadata RequestMetadata { get; }
        void SetHttpResponseHeader(string name, string value);
        void SetHttpResponseStatusCode(int statusCode);
        void InjectHttpResponseBody(string htmlSnippet);
        void WriteHttpResponse(byte[] content);
        void WriteHttpResponse(string content);
    }
}
