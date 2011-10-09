using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpsePluginMetadata
    {
        bool SessionAccessRequired { get; }
        Type RequestContextType { get; }
    }
}