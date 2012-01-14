using System;

namespace Glimpse.Core2.Extensibility
{
    public interface ITabMetadata
    {
        LifeCycleSupport LifeCycleSupport { get; }
        Type RequestContextType { get; }
    }
}