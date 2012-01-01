using System;

namespace Glimpse.Core2.Extensibility
{
    //TODO: Add contracts to ensure these values are never null
    public interface IFrameworkProvider
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        Type RuntimeContextType { get; }
        RequestMetadata RequestMetadata { get; }
        void SetHttpResponseHeader(string name, string value);
        void InjectHttpResponseBody(string htmlSnippet);
    }
}
