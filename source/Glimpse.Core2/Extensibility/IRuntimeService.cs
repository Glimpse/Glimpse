using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IRuntimeService
    {
        IDataStore HttpRequestStore { get; }
        IDataStore HttpServerStore { get; }
        object RuntimeContext { get; }
        Type RuntimeContextType { get; }
    }
}
