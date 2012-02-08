using System;

namespace Glimpse.Core2.Extensibility
{
    public interface ITab
    {
        //TODO: Create generic abstract base class to remove need for RequestContextType
        object GetData(ITabContext context);
        string Name { get; }
        LifeCycleSupport LifeCycleSupport { get; }
        Type RequestContextType { get; }
    }
}
