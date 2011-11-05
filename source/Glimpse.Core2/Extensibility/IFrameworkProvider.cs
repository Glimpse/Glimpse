using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IFrameworkProvider
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        Type RuntimeContextType { get; }
        RequestMetadata RequestMetadata { get; }
    }
}
