using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpsePluginMetadata
    {
        LifeCycleSupport LifeCycleSupport { get; }
        Type RequestContextType { get; }
    }
}