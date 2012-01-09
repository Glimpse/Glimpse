using System;

namespace Glimpse.Core2.Extensibility
{
    //TODO: Add contracts to ensure these values are never null
    //TODO: Does Http* make sense for names
    public interface IFrameworkProvider
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        //TODO: Do we really need this?
        RequestMetadata RequestMetadata { get; }
        void SetHttpResponseHeader(string name, string value);
        void SetHttpResponseStatusCode(int statusCode);
        void InjectHttpResponseBody(string htmlSnippet);
    }
}
