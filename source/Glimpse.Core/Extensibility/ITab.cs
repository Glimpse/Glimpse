using System;

namespace Glimpse.Core.Extensibility
{
    public interface ITab
    {
        object GetData(ITabContext context);
        string Name { get; }
        RuntimeEvent ExecuteOn { get; }
        Type RequestContextType { get; }
    }
}
