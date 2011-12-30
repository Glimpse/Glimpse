using System;

namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseTabMetadata
    {
        LifeCycleSupport LifeCycleSupport { get; }
        Type RequestContextType { get; }
    }
}